using System;
using System.Collections.Generic;
using System.Text;

namespace Caspara.IO
{
    public interface ISerialPort : IDisposable
    {
        event Action<object, byte[]> DataReceived;
        String Port { get; set; }
        BaudRate Baudrate { get; set; }
        ISerialPort SetBaudrate(BaudRate baudrate);
        ISerialPort SetPort(String Port);
        ISerialPort Open();
        bool IsOpened { get; }
        void Close();
        ISerialPort Write(byte[] buf);
    }
}
