using System;
using System.Collections.Generic;
using System.Text;

namespace GasSensorPackage.Messages
{
    public class GasSensorTelemetry : BaseMessage
    {
        public int PPMReading { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
