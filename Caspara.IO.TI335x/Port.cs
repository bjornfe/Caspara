using System;
using System.Diagnostics;
using System.IO;
using Caspara.IO.TI335x;

namespace Caspara.IO.TI335x
{
	/// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
	/// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
	/// <summary>
	/// Provides the base functionality for all ports

	public abstract class Port : IPort, IDisposable
	{

        public String ValuePath;

        // Track whether Dispose has been called. 
        public bool Disposed { get; protected set; } = false;

        public int Nr { get; set; } = -1;
        public virtual string Name => "Port " + Nr;

        // a flag to indicate if the port is open
        public bool PortIsOpen { get; protected set; }

        public virtual PortDirection PortDirection => PortDirection.NONE;

        public Port()
        {

        }

        ~Port()
        {
            Dispose();
        }


        public virtual void SetPortNr(int Nr)
        {
            this.Nr = Nr;
            OpenPort();
            SetSysFsDirection();
        }


        #region

        bool openedInternally = false;

		protected virtual void OpenPort()
		{
            // do the open
            try
            {
                ValuePath = Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIODIRNAMEBASE + Nr+ "/" + Definitions.SYSFS_GPIOVALUE;

                if (!System.IO.Directory.Exists(Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIODIRNAMEBASE + Nr))
                {
                    System.IO.File.WriteAllText(Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIOEXPORT, Nr.ToString());
                    openedInternally = true;
                }

            }
            catch(Exception err)
            {
                Console.WriteLine("Failed to export GPIO "+Nr+" -> "+err.ToString());
            }
		}

		/// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
		/// <summary>
		/// Closes the port. Throws an exception on failure, including if the port is
		/// already closed
		/// 
		/// This is really just doing the equivalent of a shell command 
		///    echo <gpioID> > /sys/class/gpio/unexport
		///  after which the /sys/class/gpio/gpio<gpioID> directory should not exist.

		public virtual void ClosePort()
		{
            if (openedInternally)
            {
                // do the close
                try
                {
                    if (System.IO.Directory.Exists(Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIODIRNAMEBASE + Nr))
                        System.IO.File.WriteAllText(Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIOUNEXPORT, Nr.ToString());
                }
                catch
                {
                    
                }
            }
		}

		/// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
		/// <summary>
		/// Sets the port direction at the SysFs level
		/// 
		/// This is really just doing the equivalent of a shell command 
		///    echo <direction_as_string> > /sys/class/gpio/gpio<gpioID>/direction

		protected void SetSysFsDirection()
		{
			string dirStr;

			if (PortDirection == PortDirection.INPUT)
			{
				dirStr = "in";
			} 
			else if (PortDirection == PortDirection.OUTPUT)
			{
				dirStr = "out";
			}
			else
			{
				// should never happen
				throw new Exception ("unknown port direction:" + PortDirection.ToString());
			}
            // set the direction now
            try
            {
                System.IO.File.WriteAllText(Definitions.SYSFS_GPIODIR + Definitions.SYSFS_GPIODIRNAMEBASE + Nr + "/" + Definitions.SYSFS_GPIODIRECTION, dirStr);
            }
            catch(Exception err)
            {
                Console.WriteLine("Failed to set Direction for " + Nr+" -> "+err.ToString());
            }
		}

        public bool Read()
        {
            try
            {
                var outStr = System.IO.File.ReadAllText(ValuePath);

                switch (outStr.Trim())
                {
                    case "1":
                        return true;
                    default:
                        return false;
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("Failed to read GPIONr: " + Nr + " -> " + err.ToString());
                return false;
            }

        }



        #endregion

        // #########################################################################
        // ### Dispose Code
        // #########################################################################
        #region

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Implement IDisposable. 
        /// Dispose(bool disposing) executes in two distinct scenarios. 
        /// 
        ///    If disposing equals true, the method has been called directly 
        ///    or indirectly by a user's code. Managed and unmanaged resources 
        ///    can be disposed.
        ///  
        ///    If disposing equals false, the method has been called by the 
        ///    runtime from inside the finalizer and you should not reference 
        ///    other objects. Only unmanaged resources can be disposed. 
        /// 
        ///  see: http://msdn.microsoft.com/en-us/library/system.idisposable.dispose%28v=vs.110%29.aspx
        /// 
        /// </summary>
        /// <history>
        ///    28 Aug 14 Cynic - Originally written
        /// </history>
        /// 

        public void Dispose()
        {
            if (Disposed == false)
            {
                // Call the appropriate methods to clean up 
                // unmanaged resources here. If disposing is false, 
                // only the following code is executed.

                // Clean up our port        
                ClosePort();
                // Suppress finalization.
                GC.SuppressFinalize(this);

                Disposed = true;

            }
        }

       
		#endregion

	}
}

