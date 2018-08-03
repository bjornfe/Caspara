

namespace Caspara.IO.TI335x.GPIO
{
    public class GPIOInput : Port, IDigitalInputPort
    {
        public override string Name => "Input "+Nr;
        public override PortDirection PortDirection => PortDirection.INPUT;

        public GPIOInput(int Nr)
        {
            this.Nr = Nr;
            OpenPort();
            SetSysFsDirection();
        }

        public GPIOInput()
        {

        }
    }
}
