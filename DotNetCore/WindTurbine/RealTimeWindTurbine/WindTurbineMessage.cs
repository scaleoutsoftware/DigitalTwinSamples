using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeWindTurbine
{
    /// <summary>
    /// A message class that can be sent sent to a Digital Twin instance.
    /// </summary>
    public class WindTurbineMessage
    {
        /// <summary>Device temperature.</summary>
        public double Temperature { get; set; }

        /// <summary>Device RPMs.</summary>
        public double RPM { get; set; }

        /// <summary>Timestamp of when the message was originated by device.</summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
