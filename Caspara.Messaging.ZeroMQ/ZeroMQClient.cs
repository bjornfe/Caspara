using Caspara.Messaging;
using Caspara.Persistance;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZeroMQ;

namespace Caspara.Messaging.ZeroMQ
{
    public class ZeroMQClient : IMessagePublishClient, IDisposable
    {
        ZeroMQContext context;
        ZSocket subscriber;
        ZSocket publisher;

        public String Hostname { get; set; } = "127.0.0.1";
        public int SubscribePort { get; set; } = ZeroMQConstants.ServerPublishPort;
        public int PublishPort { get; set; } = ZeroMQConstants.ServerReceivePort;

        private bool Active = true;
        private bool Running = false;

        public event EventHandler<string> MessageReceived;

        public ZeroMQClient()
        {
            context = new ZeroMQContext();
            subscriber = context.CreateClientSocket(ZSocketType.SUB);
            publisher = context.CreateClientSocket(ZSocketType.PUB);
        }

        public IMessagePublishClient Start()
        {
            if (Hostname == null || Hostname.Equals(""))
                Hostname = "127.0.0.1";

            if (SubscribePort == 0)
                SubscribePort = ZeroMQConstants.ServerPublishPort;

            if (PublishPort == 0)
                PublishPort = ZeroMQConstants.ServerReceivePort;

            var subscribeString = "tcp://" + Hostname + ":" + SubscribePort;
            var publishString = "tcp://" + Hostname + ":" + PublishPort;

            Console.WriteLine("Publisher  -> " + publishString);
            Console.WriteLine("Subscriber -> " + subscribeString);

            //Console.WriteLine("Connecting to " + connectionString);
            subscriber.Connect(subscribeString, out var connectError);
            if(connectError != null)
            {
                Console.WriteLine($"Connection error: {connectError.Name} - {connectError.Number} - {connectError.Text}");
            }

            publisher.Connect(publishString, out var connectError2);
            if (connectError != null)
            {
                Console.WriteLine($"Connection error: {connectError2.Name} - {connectError2.Number} - {connectError2.Text}");
            }

            new Thread(new ThreadStart(() =>
            {
                Running = true;
                while (Active)
                {
                    try
                    {
                        using (ZMessage message = subscriber.ReceiveMessage(out var error))
                        {
                            if (message != null)
                            {
                                var topic = message[0].ReadString();
                                var msg = message[1].ReadString();
                                MessageReceived?.Invoke(topic, msg);
                            }
                            else
                            {
                                Thread.Sleep(10);
                            }
                        }
                    }
                    catch
                    {
                       
                    }
                }
                Running = false;
            })).Start();

           

            return this;

        }

        public IMessagePublishClient Stop()
        {
            try
            {
                Active = false;
                

                context.Dispose();

            }
            catch
            {

            }
            return this;
        }

        public IMessagePublishClient Subscribe(string Topic)
        {
            subscriber.Subscribe(Topic);
            return this;
        }

        public IMessagePublishClient Unsubscribe(string Topic)
        {
            subscriber.Unsubscribe(Topic);
            return this;
        }

        public void Dispose()
        {
            Stop();
        }

        public IMessagePublishClient Publish(string Topic, string Message)
        {
            using(var msg = new ZMessage())
            {
                try
                {
                    msg.Add(new ZFrame(Topic));
                    msg.Add(new ZFrame(Message));
                    publisher.Send(msg);
                }
                catch
                {

                }
            }
            return this;
        }
    }
}
