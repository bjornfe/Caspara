using Caspara.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZeroMQ;

namespace Caspara.Messaging.ZeroMQ
{
    public class ZeroMQP2PListener : IMessageP2PListener
    {
        ZeroMQContext Context;
        ZSocket Listener;

        bool Active = true;
        bool Running = false;

        Thread runningThread;

        public ZeroMQP2PListener()
        {
            Context = new ZeroMQContext();
            Listener = Context.CreateServerSocket(ZSocketType.REP);
        }

        public int ListenPort { get; set; }

        public event Func<string, string> HandleMessage;

        public void Start()
        {
            Listener.Bind("tcp://*:" + ListenPort, out var connectError);
            if (connectError != null)
            {
                Console.WriteLine($"Connection error: {connectError.Name} - {connectError.Number} - {connectError.Text}");
            }

            runningThread = new Thread(new ThreadStart(() =>
            {
                Running = true;
                while (Active)
                {
                    try
                    {
                        var message = Listener.ReceiveMessage(out var error);
                        if (error == null)
                        {
                            if (message != null && message.Count > 0)
                            {
                                try
                                {
                                    if (HandleMessage != null)
                                    {
                                        var responseMessage = HandleMessage?.Invoke(message[0].ReadString());
                                        using (ZMessage reponse = new ZMessage())
                                        {
                                            reponse.Add(new ZFrame(responseMessage));
                                            Listener.Send(reponse);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ZeroMQP2PListener: MessageHandler not defined");
                                    }
                                }
                                catch(Exception err)
                                {
                                    Console.WriteLine("Failed to Receive P2P Message -> " + err.ToString());
                                }
                            }
                        }
                    }
                    catch(Exception err)
                    {
                        Console.WriteLine("MessageListener failed -> " + err.ToString());
                    }

                    Thread.Sleep(10);
                }
                Running = false;
            }));
            runningThread.Start();
        }

        public void Stop()
        {
            Active = false;
            Context.Dispose();
            runningThread.Join();
        }
    }
}
