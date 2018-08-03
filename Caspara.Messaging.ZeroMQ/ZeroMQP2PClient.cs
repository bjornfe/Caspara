using System;
using System.Collections.Generic;
using System.Text;
using ZeroMQ;

namespace Caspara.Messaging.ZeroMQ
{
    public class ZeroMQP2PClient : IMessageP2PClient
    {
        public string GetResponse(string Host, int Port, string Message, int TimeOutSeconds = 10)
        {
            using(var Context = new ZContext())
            {
                using(var Client = new ZSocket(Context, ZSocketType.REQ))
                {
                    Client.Connect("tcp://" + Host + ":" + Port, out var connectError);
                    Client.ReceiveTimeout = TimeSpan.FromSeconds(TimeOutSeconds);
                    if (connectError != null)
                    {
                        Console.WriteLine($"Connection error: {connectError.Name} - {connectError.Number} - {connectError.Text}");
                        return null;
                    }
                    else
                    {
                        using(var request = new ZFrame(Message))
                        {
                            Client.Send(request);
                            try
                            {
                                using (var reponse = Client.ReceiveFrame(out var error))
                                {
                                    if(error == null)
                                        return reponse.ReadString();
                                    else
                                    {
                                        Console.WriteLine($"Reponse error: {error.Name} - {error.Number} - {error.Text}");
                                    }
                                }
                                
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }

            return null;
        }


    }
}
