using System;
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    public class WindTurbineMessage : DigitalTwinMessage
    {
        /// <summary>Device temperature.</summary>
        public double Temperature { get; set; }

        /// <summary>Device RPMs.</summary>
        public double RPM { get; set; }

        /// <summary>Timestamp of when the message was originated by device.</summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// Overridden property used by JsonSubtypes during deserialization.
        /// </summary>
        public override string MessageType => nameof(WindTurbineMessage);
    }
}
