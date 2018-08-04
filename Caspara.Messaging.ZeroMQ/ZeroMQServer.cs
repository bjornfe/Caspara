using Caspara.Messaging;
using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZeroMQ;
using ZeroMQ.Devices;

namespace Caspara.Messaging.ZeroMQ
{
    public class ZeroMQServer : IMessagePublishServer, IDisposable
    {
        ZeroMQContext context;
        ZSocket publisher;
        ZSocket subscriber;
        //PubSubDevice device;

        public int SubscribePort { get; set; }
        public int PublishPort { get; set; }

        private bool Active = true;
        private bool Running = false;

        Thread runningThread;

        public void Start()
        {

            if (SubscribePort == 0)
                this.SubscribePort = ZeroMQConstants.ServerReceivePort;

            if (PublishPort == 0)
                this.PublishPort = ZeroMQConstants.ServerPublishPort;


            context = new ZeroMQContext();

            publisher = context.CreateServerSocket(ZSocketType.PUB);
            subscriber = context.CreateServerSocket(ZSocketType.SUB);

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
            runningThread = new Thread(new ThreadStart(() =>
            {
                Running = true;
                while (Active)
                {
                    try
                    {
                        using (ZMessage message = subscriber.ReceiveMessage(out var error))
                        {
                            
                            if (error == null && message != null && message.Count > 0)
                            {
                                publisher.Send(message);
                            }
                            else
                            {
                                Thread.Sleep(1);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                Running = false;
            }));
            runningThread.Start();
        }

        public void Stop()
        {
            Active = false;
            context.Dispose();
            runningThread.Join();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
