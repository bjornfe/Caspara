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
        ZContext Context;
        ZSocket Listener;

        bool Active = true;
        bool Running = false;

        public ZeroMQP2PListener()
        {
            Context = new ZContext();

            Listener = new ZSocket(Context, ZSocketType.REP);

            Z85.CurveKeypair(out byte[] publicSubscribeKey, out byte[] privateSubscribeKey);
            Listener.CurvePublicKey = ZeroMQConstants.GetServerPublicKey();
            Listener.CurveSecretKey = ZeroMQConstants.GetServerPrivateKey();
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

            new Thread(new ThreadStart(() =>
            {
                Running = true;
                while (Active)
                {
                    try
                    {
                        using (ZMessage message = Listener.ReceiveMessage(ZSocketFlags.DontWait))
                        {
                            if(message != null)
                            {
                                var responseMessage = HandleMessage?.Invoke(message[0].ReadString());
                                using(ZMessage reponse = new ZMessage())
                                {
                                    reponse.Add(new ZFrame(responseMessage));
                                    Listener.Send(reponse);
                                } 
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
        }

        public void Stop()
        {
            Active = false;
            while (Running)
                Thread.Sleep(10);

            Listener.Close();
            Listener.Dispose();
            Context.Dispose();

        }
    }
}
