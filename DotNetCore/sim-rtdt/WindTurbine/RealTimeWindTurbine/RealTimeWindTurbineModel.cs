using System;
using System.Collections.Generic;

using Scaleout.Streaming.DigitalTwin.Core;
using WindTurbineMessages;

namespace RealTimeWindTurbine
{
    /// <summary>
    /// Model holding that holds state for a simulated 
    /// digital twin instance.
    /// </summary>
    public class RealTimeWindTurbineModel : DigitalTwinBase
    {
        public int Temperature { get; set; }
    }
}
