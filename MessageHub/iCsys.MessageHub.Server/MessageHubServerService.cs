using iCsys.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace iCsys.MessageHub.Server
{
    public class MessageHubServerService<T> : ThreadService, IMessageHubServerService<T> where T : IHubMessage
    {
        protected TcpListener Listener;
        protected IPAddress ListenerIP;
        protected int ListenerPort;
        protected List<HubClientConnection<T>> Connections = new List<HubClientConnection<T>>();
        protected Dictionary<String, List<HubClientConnection<T>>> Subscriptions = new Dictionary<string, List<HubClientConnection<T>>>();

        private readonly object _lockObject = new object();

        public override string Name { get; set; } = "MessageHub Server"; 

        public MessageHubServerService(IPAddress ListenerIP, int ListenerPort)
        {
            this.ListenerIP = ListenerIP;
            this.ListenerPort = ListenerPort;
            this.SleepInterval = 10;
        }

        public override void Start()
        {
            Listener = new TcpListener(ListenerIP, ListenerPort);
            Listener.Start();
            base.Start();
        }

        public override void Stop()
        {
            Listener.Stop();
            base.Stop();
        }

        public override void PerformAction()
        {
            if (Listener.Pending())
            {
                lock (_lockObject)
                {
                    var connection = new HubClientConnection<T>(Listener.AcceptTcpClient());
                    connection.MessageReceived += Client_MessageReceived;
                    connection.Disconnected += Client_Disconnected;
                    Connections.Add(connection);
                }

            }

            lock (_lockObject)
            {
                foreach (var connection in Connections)
                {
                    Task.Run(connection.ReadAndWrite);
                }
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            lock (_lockObject)
            {
                if (sender is HubClientConnection<T> client)
                {
                    HandleDisconnect(client);
                    Connections.Remove(client);
                }
            }
        }

        private void Client_MessageReceived(object sender, T e)
        {
            if (sender is HubClientConnection<T> client)
                ProcessMessage(client, e);
        }

        public virtual void HandleDisconnect(HubClientConnection<T> Client)
        {
            lock (_lockObject)
            {
                while (Subscriptions.Count > 0 && Subscriptions.Values.Any(v => v.Contains(Client)))
                {
                    var entry = Subscriptions.Where(v => v.Value.Contains(Client)).First();
                    entry.Value.Remove(Client);
                    if (entry.Value.Count == 0)
                        Subscriptions.Remove(entry.Key);

                }
            }
        }

        public virtual void ProcessMessage(HubClientConnection<T> Client, T Message)
        {
            if (Message != null)
            {
                lock (_lockObject)
                {
                    switch (Message.MessageType)
                    {
                        case HubMessageType.Subscribe:
                            if (!Subscriptions[Message.Topic].Contains(Client))
                                Subscriptions[Message.Topic].Add(Client);
                            break;
                        case HubMessageType.Unsubscribe:
                            if (Subscriptions[Message.Topic].Contains(Client))
                                Subscriptions[Message.Topic].Remove(Client);
                            break;
                        case HubMessageType.Forward:
                            if (Subscriptions.ContainsKey(Message.Topic))
                                foreach (var c in Subscriptions[Message.Topic])
                                    c.Send(Message);
                            break;
                    }
                }
            }
        }
    }
}
