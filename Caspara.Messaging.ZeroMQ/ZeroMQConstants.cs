using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Caspara.Messaging.ZeroMQ
{
    public static class ZeroMQConstants
    {
        public const int ServerReceivePort = 65002;
        public const int ServerPublishPort = 65003;

        public const string SERVER_PUBLICKEY = "583D5D396B4F43353157777B7D253F6C25535757517D28506A2856347423616D396E6C4A59425D28";
        public const string SERVER_PRIVATEKEY = "4A5E3F7A304F5A4C4B554450585646384444584444263A5A2976232875446976486C4C3F5235254F";

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static byte[] GetServerPublicKey()
        {
            return StringToByteArray(SERVER_PUBLICKEY);
        }

        public static byte[] GetServerPrivateKey()
        {
            return StringToByteArray(SERVER_PRIVATEKEY);
        }
    }
}
