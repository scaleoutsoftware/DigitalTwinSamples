using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindTurbineMessages
{
    /// <summary>
    /// Message sent from a real-time digital twin to a device or, in this case,
    /// a simulation digital twin like the SimulatedWindTurbine project.
    /// </summary>
    public class DeviceCommandMessage
    {
        /// <summary>
        /// Command to issue to a device. The simulated wind turbine supports
        /// "start" and "shutdown".
        /// </summary>
        public string CommandText { get; set; }
    }
}
