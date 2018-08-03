using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Caspara.IO.TI335x.GPIO;

namespace Caspara.IO.TI335x.ADC
{
    public class AnalogueInputPort : Port, IAnalogueInputPort
    {

        // the stream descriptor for the open a2d file
        Stream a2dStream = null;
        StreamReader a2dReader = null;

        public override PortDirection PortDirection => PortDirection.INPUT;
        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Constructor
        /// </summary>

        public AnalogueInputPort()
        {

        }

        public AnalogueInputPort(int nr)
        {
            SetPortNr(nr);
        }

        public override void SetPortNr(int nr)
        {
            bool valid = true;

            if (nr > -1 && nr < 7)
                Nr = nr;
            else
                valid = false;

            if (valid)
                OpenPort();
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Reads the current value in from an A2D Device. 
        /// 
        /// NOTE: 
        ///   Tests indicate that the maximum possible speed here is about 
        ///   1000 reads/second
        ///   
        /// </summary>
        public double ReadValue()
        {
            string line;

            if (a2dStream == null)
            {
                throw new Exception("A2D port is not open, a2dStream=null");
            }
            if (PortIsOpen == false)
            {
                throw new Exception("A2D port is not open");
            }

            // reset our location
            a2dStream.Seek(0, SeekOrigin.Begin);
            line = a2dReader.ReadLine();
            //Console.WriteLine("A2DPort read returned "+ line);
            return Convert.ToUInt32(line);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Opens the port. Throws an exception on failure
        /// 
        /// </summary>
        protected override void OpenPort()
        {
            string deviceFileName;
            // set up now
            deviceFileName = Definitions.A2DIIO_FILENAME;

            if(Nr > -1 && Nr < 7)
                deviceFileName = deviceFileName.Replace("%port%", Nr.ToString());
            else
                throw new Exception("Unknown A2D Port:" + Nr.ToString());


            // we open the file.
            a2dStream = File.Open(deviceFileName, FileMode.Open);
            if (a2dStream == null)
            {
                throw new Exception("Could not open a2d device file:" + deviceFileName);
            }
            // we open the reader
            a2dReader = new StreamReader(a2dStream);
            PortIsOpen = true;

            // Console.WriteLine("A2DPort Port Device Enabled: "+ deviceFileName);
        }

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Closes the port. 
        /// 
        /// </summary>

        public override void ClosePort()
        {
            //Console.WriteLine("A2DPort Closing");
            if (a2dReader != null)
            {
                // do a close
                a2dReader.Close();
            }
            if (a2dStream != null)
            {
                // do a close
                a2dStream.Close();
            }
            PortIsOpen = false;
            a2dStream = null;
            a2dReader = null;
        }
    }
}
