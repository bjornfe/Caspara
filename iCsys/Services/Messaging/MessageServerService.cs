using Caspara.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Caspara.Services.Messaging.Server
{
    public class MessageServerService<T> : ThreadService, IMessageServerService<T> where T : IMessage
    {
        protected TcpListener Listener;
        protected IPAddress ListenerIP;
        protected int ListenerPort;
        protected List<MessageConnection<T>> Connections = new List<MessageConnection<T>>();
        protected Dictionary<String, List<MessageConnection<T>>> Subscriptions = new Dictionary<string, List<MessageConnection<T>>>();

        private readonly object _lockObject = new object();

        public override string Name { get; set; } = "MessageHub Server"; 

        public MessageServerService(IPAddress ListenerIP, int ListenerPort)
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
                    var connection = new MessageConnection<T>(Listener.AcceptTcpClient());
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
                if (sender is MessageConnection<T> client)
                {
                    HandleDisconnect(client);
                    Connections.Remove(client);
                }
            }
        }

        private void Client_MessageReceived(object sender, T e)
        {
            if (sender is MessageConnection<T> client)
                ProcessMessage(client, e);
        }

        public virtual void HandleDisconnect(MessageConnection<T> Client)
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

        public virtual void ProcessMessage(MessageConnection<T> Client, T Message)
        {
            if (Message != null)
            {
                lock (_lockObject)
                {
                    switch (Message.MessageType)
                    {
                        case MessageType.Subscribe:
                            if (!Subscriptions[Message.Topic].Contains(Client))
                                Subscriptions[Message.Topic].Add(Client);
                            break;
                        case MessageType.Unsubscribe:
                            if (Subscriptions[Message.Topic].Contains(Client))
                                Subscriptions[Message.Topic].Remove(Client);
                            break;
                        case MessageType.Forward:
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
