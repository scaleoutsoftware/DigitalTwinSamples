using System;

namespace WindTurbineMessages
{
    public class WindTurbineMessage
    {
        /// <summary>Device location's latitude.</summary>
        public decimal Latitude { get; set; }

        /// <summary>Device location's longitude.</summary>
        public decimal Longitude { get; set; }

        /// <summary>Timestamp of when the message was originated by device.</summary>
        public DateTime Timestamp { get; set; }

        // ...
        // Other properties
        // ...
    }
}
