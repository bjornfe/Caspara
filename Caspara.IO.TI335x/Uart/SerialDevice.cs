using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Caspara.IO.TI335x.Uart
{
    public class SerialDevice : IDisposable, ISerialPort
    {
        public const int READING_BUFFER_SIZE = 1024;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private CancellationToken CancellationToken => cts.Token;

        private int? fd;
        private readonly IntPtr readingBuffer = Marshal.AllocHGlobal(READING_BUFFER_SIZE);

        public String Port { get; set; }

        public BaudRate Baudrate { get; set; }

        public event Action<object, byte[]> DataReceived;
        public SerialDevice()
        {

        }

        public ISerialPort Open()
        {
            // open serial port
            int fd = Libc.open(Port, Libc.OpenFlags.O_RDWR | Libc.OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                throw new Exception($"failed to open port ({Port})");
            }

            // set baud rate
            byte[] termiosData = new byte[256];

            Libc.tcgetattr(fd, termiosData);
            Libc.cfsetspeed(termiosData, Baudrate);
            Libc.tcsetattr(fd, 0, termiosData);
            // start reading
            Task.Run((Action)StartReading, CancellationToken);
            this.fd = fd;
            return this;
        }

        private void StartReading()
        {
            if (!fd.HasValue)
            {
                throw new Exception();
            }

            while (true)
            {
                CancellationToken.ThrowIfCancellationRequested();

                int res = Libc.read(fd.Value, readingBuffer, READING_BUFFER_SIZE);

                if (res != -1)
                {
                    byte[] buf = new byte[res];
                    Marshal.Copy(readingBuffer, buf, 0, res);

                    OnDataReceived(buf);
                }

                Thread.Sleep(50);
            }
        }

        protected virtual void OnDataReceived(byte[] data)
        {
            DataReceived?.Invoke(this, data);
        }

        public bool IsOpened => fd.HasValue;

        public void Close()
        {
            if (!fd.HasValue)
            {
                throw new Exception();
            }

            cts.Cancel();
            Libc.close(fd.Value);
            Marshal.FreeHGlobal(readingBuffer);
        }

        public ISerialPort Write(byte[] buf)
        {
            if (!fd.HasValue)
            {
                throw new Exception();
            }

            IntPtr ptr = Marshal.AllocHGlobal(buf.Length);
            Marshal.Copy(buf, 0, ptr, buf.Length);
            Libc.write(fd.Value, ptr, buf.Length);
            Marshal.FreeHGlobal(ptr);
            return this;
        }

        public void Dispose()
        {
            if (IsOpened)
            {
                Close();
            }
        }

        public ISerialPort SetBaudrate(BaudRate baudrate)
        {
            Baudrate = baudrate;
            return this;
        }

        public ISerialPort SetPort(string Port)
        {
            this.Port = Port;
            return this;
        }

    }
}
