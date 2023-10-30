using System;
using JsonSubTypes;
using Newtonsoft.Json;

namespace Messages
{
    /// <summary>
    /// Base class for messages sent to a real-time digital twin.
    /// </summary>
    /// <remarks>
    /// Messages sent to real-time digitial twins should derive from this type.
    /// </remarks>
    [JsonConverter(typeof(JsonSubtypes), "MessageType")]
    public abstract class DigitalTwinMessage
    {
        /// <summary>
        /// Used during deserialization to determine derived message type.
        /// </summary>
        /// <see href="https://static.scaleoutsoftware.com/docs/digital_twin_user_guide/software_toolkit/dt_builder/dotnetcore_api/dotnetcore_multiple_msg_types.html">Using Multiple Message Types</see>
        public abstract string MessageType { get; }
    }
}
