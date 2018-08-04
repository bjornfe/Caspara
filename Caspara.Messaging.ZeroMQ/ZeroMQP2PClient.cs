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
            var Context = new ZeroMQContext();
            var Client = Context.CreateClientSocket(ZSocketType.REQ);
            Client.ReceiveTimeout = TimeSpan.FromSeconds(TimeOutSeconds);
            Client.Connect("tcp://" + Host + ":" + Port, out var connectError);

            string responseString = "";

            if (connectError != null)
            {
                Console.WriteLine($"Connection error: {connectError.Name} - {connectError.Number} - {connectError.Text}");
                return null;
            }
            else
            {
                var requestMessage = new ZMessage();
                requestMessage.Add(new ZFrame(Message));
                Client.Send(requestMessage, out var sendError);
                if (sendError == null)
                {
                    
                    try
                    {
                        ZMessage response = null;
                        bool timeout = false;
                        DateTime sendTime = DateTime.Now;
                        while (response == null && !timeout)
                        {
                            response = Client.ReceiveMessage(out var error);
                            if (error != null)
                            {
                                timeout = true;
                                Console.WriteLine($"ZeroMQ P2P Client ReadResponse error: {error.Name} - {error.Number} - {error.Text}");
                            }
                            else
                                timeout = (DateTime.Now - sendTime).TotalSeconds > TimeOutSeconds;
                        }

                        if (response != null)
                        {
                            responseString = response[0].ReadString();
                        }
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Failed -> " + err.ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"Send error: {sendError.Name} - {sendError.Number} - {sendError.Text}");
                }
                requestMessage.Dispose();

            }

            Context.Dispose();

            return responseString;

        }
    }
}
