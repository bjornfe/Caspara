using System;

namespace Caspara.IO.TI335x.GPIO
{
	/// <summary>
	/// Provides the Output Port functionality for a TI335x
	/// </summary>
	public class GPIOOutput : Port, IDigitalOutputPort
	{
        public override string Name => "Output "+Nr;
        public override PortDirection PortDirection => PortDirection.OUTPUT;

        public GPIOOutput(int Nr)
        {
            SetPortNr(Nr);
        }

        public GPIOOutput()
        {
        }

        public void Write(bool valueToSet)
		{
            try
            {
                string valueStr;
                if (valueToSet == true)
                {
                    valueStr = "1";
                }
                else
                {
                    valueStr = "0";
                }
                // set the value now        
                System.IO.File.WriteAllText(Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIODIRNAMEBASE + Nr + "/" + Definitions.SYSFS_GPIOVALUE, valueStr);
            }
            catch(Exception err)
            {
                Console.WriteLine("Failed to write to GPIO " + Nr + " -> " + err.ToString());
            }
        }

	}
}

