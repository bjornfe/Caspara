using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace iCsys.MessageHub
{
    public class HubClientConnection<T> : IDisposable where T : IHubMessage
    {
        public event EventHandler<T> MessageReceived;
        public event EventHandler Disconnected;
        bool Active = true;
        bool Running = false;
        TcpClient Client;
        NetworkStream NetworkStream;
        StreamWriter Writer;
        StreamReader Reader;

        ConcurrentQueue<T> SendMessageQueue = new ConcurrentQueue<T>();

        public void Send(T Message)
        {
            SendMessageQueue.Enqueue(Message);
        }

        public Action ReadAndWrite => () =>
        {
            if (Client.Connected)
            {
                if (Client.Available > 0)
                {
                    try
                    {
                        var message = Reader.ReadLineAsync();
                        var msg = JsonConvert.DeserializeObject<T>(message.Result);
                        if(msg != null)
                            MessageReceived?.Invoke(this, msg);
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("Failed to read message from client -> " + err.ToString());
                    }
                }

                while (SendMessageQueue.Count > 0)
                {
                    if (SendMessageQueue.TryDequeue(out T sendMessage))
                    {
                        try
                        {
                            var msg = JsonConvert.SerializeObject(sendMessage);
                            Writer.WriteLineAsync(msg);
                        }
                        catch (Exception err)
                        {
                            Console.WriteLine("Failed to write message to client -> " + err.ToString());
                        }
                    }

                }
            }
            else
            {
                Disconnected?.Invoke(this, null);
                Dispose();
            }
        };

        public HubClientConnection(TcpClient Client)
        {
            this.Client = Client;
            NetworkStream = Client.GetStream();
            Writer = new StreamWriter(NetworkStream);
            Reader = new StreamReader(NetworkStream);
        }

        public void SendMessage(String Message)
        {
            Writer.WriteLine(Message);
        }

        public void Dispose()
        {
            Reader.Dispose();
            Writer.Dispose();
            NetworkStream.Dispose();
            Client.Dispose();
        }
    }
}
