using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZeroMQ;

namespace Caspara.Messaging.ZeroMQ
{
    public class ZeroMQContext : IDisposable
    {
        private string SERVER_PUBLICKEY = "583D5D396B4F43353157777B7D253F6C25535757517D28506A2856347423616D396E6C4A59425D28";
        private string SERVER_PRIVATEKEY = "4A5E3F7A304F5A4C4B554450585646384444584444263A5A2976232875446976486C4C3F5235254F";
        private byte[] ServerPublicKey => StringToByteArray(SERVER_PUBLICKEY);
        private byte[] ServerPrivateKey => StringToByteArray(SERVER_PRIVATEKEY);


        List<ZSocket> Sockets = new List<ZSocket>();
        ZContext context;
        public ZeroMQContext()
        {
            context = new ZContext();
        }

        public ZSocket CreateClientSocket(ZSocketType Type)
        {
            var s = new ZSocket(context, Type);
            s.SetOption(ZSocketOption.CONFLATE, 1);
            Z85.CurveKeypair(out byte[] publicKey, out byte[] privateKey);
            s.CurvePublicKey = publicKey;
            s.CurveSecretKey = privateKey;
            s.CurveServerKey = ServerPublicKey;
            Sockets.Add(s);
            return s;
        }

        public ZSocket CreateServerSocket(ZSocketType Type)
        {
            var s = new ZSocket(context, Type);
            s.SetOption(ZSocketOption.CONFLATE, 1);
            Z85.CurveKeypair(out byte[] publicKey, out byte[] privateKey);
            s.CurveServer = true;
            s.CurvePublicKey = ServerPublicKey;
            s.CurveSecretKey = ServerPrivateKey;
            Sockets.Add(s);
            return s;
        }

        public void Dispose()
        {
            foreach(var s in Sockets)
            {
                s.SetOption(ZSocketOption.LINGER, 0);
                s.Close();
                s.Dispose();
            }
            context.Shutdown();
            context.Terminate();
            context.Dispose();
        }

        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        

    }
}
