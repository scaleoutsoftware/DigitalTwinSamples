using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GasSensorPackage.Messages
{
    /// <summary>
    /// Base class for all messages sent to the real-time digital twin instance.
    /// </summary>
    [JsonDerivedType(typeof(BaseMessage), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(GasSensorTelemetry), typeDiscriminator: "gasSensorTelemetry")]
    [JsonDerivedType(typeof(DeviceCommand), typeDiscriminator: "deviceCommand")]
    public class BaseMessage
    {
    }
}
