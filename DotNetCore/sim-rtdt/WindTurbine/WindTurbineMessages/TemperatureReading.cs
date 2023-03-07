using System;

namespace WindTurbineMessages
{
    public class TemperatureReading
    {
        public int Temperature { get; set; }

        /// <summary>Timestamp of when the message was originated by device.</summary>
        public DateTime TimestampUtc { get; set; }

    }
}
