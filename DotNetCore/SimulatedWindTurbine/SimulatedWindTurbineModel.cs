using Scaleout.Streaming.DigitalTwin.Core;

namespace SimulatedWindTurbine
{
    public class SimulatedWindTurbineModel : DigitalTwinBase
    {
        public double CurrentTemp { get; set; } 

        public double CurrentRPMs { get; set; }
    }
}
