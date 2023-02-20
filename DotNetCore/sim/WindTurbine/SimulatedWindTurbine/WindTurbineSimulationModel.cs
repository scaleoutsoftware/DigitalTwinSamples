using System;
using System.Collections.Generic;

using Scaleout.Streaming.DigitalTwin.Core;
using WindTurbineMessages;

namespace SimulatedWindTurbine
{
    public class WindTurbineSimulationModel : DigitalTwinBase
    {
        public const string DigitalTwinModelName = "my_dt_model";

        /// The list of processed messages by this digital twin.
        public List<WindTurbineMessage> MessageList { get; } = new List<WindTurbineMessage>();

        // ...
        // Add class members that track the data source's state and implement 
        // message-processing logic.
        // ...
    }
}
