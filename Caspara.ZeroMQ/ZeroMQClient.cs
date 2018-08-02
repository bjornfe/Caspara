using Caspara.Messaging;
using Caspara.Persistance;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZeroMQ;

namespace Caspara.ZeroMQ
{
    public class ZeroMQClient : IMessageClient, IDisposable
    {
        ZContext context;
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
            context = new ZContext();

            subscriber = new ZSocket(context, ZSocketType.SUB);
            publisher = new ZSocket(context, ZSocketType.PUB);

            Z85.CurveKeypair(out byte[] publicSubscribeKey, out byte[] privateSubscribeKey);
            subscriber.CurvePublicKey = publicSubscribeKey;
            subscriber.CurveSecretKey = privateSubscribeKey;
            subscriber.CurveServerKey = ZeroMQConstants.GetServerPublicKey();

            Z85.CurveKeypair(out byte[] publicPublishKey, out byte[] privatePublishKey);
            publisher.CurvePublicKey = publicPublishKey;
            publisher.CurveSecretKey = privatePublishKey;
            publisher.CurveServerKey = ZeroMQConstants.GetServerPublicKey();
        }

        public IMessageClient Start()
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
                        using (ZMessage message = subscriber.ReceiveMessage())
                        {
                            var topic = message[0].ReadString();
                            var msg = message[1].ReadString();
                            MessageReceived?.Invoke(topic, msg);
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

        public IMessageClient Stop()
        {
            try
            {
                Active = false;

                subscriber.Close();
                publisher.Close();

                subscriber.Dispose();
                publisher.Dispose();

                context.Dispose();
            }
            catch
            {

            }
            return this;
        }

        public IMessageClient Subscribe(string Topic)
        {
            subscriber.Subscribe(Topic);
            return this;
        }

        public IMessageClient Unsubscribe(string Topic)
        {
            subscriber.Unsubscribe(Topic);
            return this;
        }

        public void Dispose()
        {
            Stop();
        }

        public IMessageClient Publish(string Topic, string Message)
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
