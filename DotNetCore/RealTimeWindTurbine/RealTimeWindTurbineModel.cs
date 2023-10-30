using Messages;
using System;
using Scaleout.Streaming.DigitalTwin.Core;
using System.Collections.Generic;

namespace RealTimeWindTurbine
{
    public class RealTimeWindTurbineModel : DigitalTwinBase
    {
        public List<double> GearboxTemps { get; } = new List<double>();

        public List<double> RPMs { get; } = new List<double>();
    }
}
