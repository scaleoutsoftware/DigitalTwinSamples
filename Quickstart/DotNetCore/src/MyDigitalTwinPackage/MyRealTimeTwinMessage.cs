using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDigitalTwinPackage
{
    /// <summary>
    /// An example message class used to demonstrate how to process messages
    /// sent to a Digital Twin instance. This class can be removed and replaced
    /// with your own message class that represents the data you expect to receive.
    /// </summary>
    public class MyRealTimeTwinMessage
    {
        public string? StringPayload { get; set; }
    }
}
