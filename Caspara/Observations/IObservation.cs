

using Caspara.Values;

namespace Caspara.Observations
{
    public interface IObservation : IObject
    {
        IValue Value { get; set; }
    }
}
