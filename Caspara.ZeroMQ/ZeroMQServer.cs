using Caspara.Messaging;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZeroMQ;
using ZeroMQ.Devices;

namespace Caspara.ZeroMQ
{
    public class ZeroMQServer : IMessageServer, IDisposable
    {
        ZContext context;
        ZSocket publisher;
        ZSocket subscriber;
        //PubSubDevice device;

        public int SubscribePort { get; set; }
        public int PublishPort { get; set; }

        private bool Active = true;
        private bool Running = false;

        

        public void Start()
        {

            if (SubscribePort == 0)
                this.SubscribePort = ZeroMQConstants.ServerReceivePort;

            if (PublishPort == 0)
                this.PublishPort = ZeroMQConstants.ServerPublishPort;


            context = new ZContext();

            publisher = new ZSocket(context, ZSocketType.PUB);
            subscriber = new ZSocket(context, ZSocketType.SUB);

            publisher.CurveServer = true;
            publisher.CurvePublicKey = ZeroMQConstants.GetServerPublicKey();
            publisher.CurveSecretKey = ZeroMQConstants.GetServerPrivateKey();

            subscriber.CurveServer = true;
            subscriber.CurvePublicKey = ZeroMQConstants.GetServerPublicKey();
            subscriber.CurveSecretKey = ZeroMQConstants.GetServerPrivateKey();

            var publisherString = "tcp://*:" + PublishPort;
            var subscriberString = "tcp://*:" + SubscribePort;

            Console.WriteLine("Publisher  -> " + publisherString);
            Console.WriteLine("Subscriber -> " + subscriberString);

            publisher.Bind(publisherString, out var connectError);
            if (connectError != null)
            {
                Console.WriteLine($"Connection error: {connectError.Name} - {connectError.Number} - {connectError.Text}");
            }

            subscriber.Bind(subscriberString, out var connect2Error);
            if (connect2Error != null)
            {
                Console.WriteLine($"Connection error: {connect2Error.Name} - {connect2Error.Number} - {connect2Error.Text}");
            }

            subscriber.SubscribeAll();
            new Thread(new ThreadStart(() =>
            {
                Running = true;
                while (Active)
                {
                    try
                    {
                        using (ZMessage message = subscriber.ReceiveMessage())
                        {
                            Console.WriteLine("Received Message -> " + message[0].ReadString());
                            publisher.Send(message);
                        }
                    }
                    catch
                    {

                    }
                }
                Running = false;
            })).Start();
        }

        public void Stop()
        {
            Active = false;

            subscriber.Close();
            publisher.Close();

            subscriber.Dispose();
            publisher.Dispose();

            context.Dispose();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
