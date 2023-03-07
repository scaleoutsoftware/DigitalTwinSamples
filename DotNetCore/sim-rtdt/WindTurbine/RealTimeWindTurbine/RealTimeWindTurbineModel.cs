using System;
using System.Collections.Generic;

using Scaleout.Streaming.DigitalTwin.Core;
using WindTurbineMessages;

namespace RealTimeWindTurbine
{
    public class RealTimeWindTurbineModel : DigitalTwinBase
    {
        public int Temperature { get; set; }
    }
}
