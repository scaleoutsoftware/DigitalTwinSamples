using System;
using System.Collections.Generic;
using System.Text;

namespace GasSensorPackage.Messages
{
    public class DeviceCommand : BaseMessage
    {
        public string? Description { get; set; }
        public int Code { get; set; }
    }
}
