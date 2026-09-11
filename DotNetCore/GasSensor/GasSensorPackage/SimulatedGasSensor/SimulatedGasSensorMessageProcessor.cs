using GasSensorPackage.Messages;
using Microsoft.Extensions.Logging.Abstractions;
using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasSensorPackage.SimulatedGasSensor
{
    public class SimulatedGasSensorMessageProcessor : MessageProcessor<SimulatedGasSensorModel>
    {
        ILogger<SimulatedGasSensorMessageProcessor> _logger;

        /// <summary>
        /// Message handler constructor. Parameters are supplied via Dependency Injection
        /// and can be modified as needed.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        public SimulatedGasSensorMessageProcessor(ILogger<SimulatedGasSensorMessageProcessor>? logger)
        {
            _logger = logger ?? NullLogger<SimulatedGasSensorMessageProcessor>.Instance;
        }


        /// <summary>
        /// Processes a message sent to the specified digital twin instance.
        /// </summary>
        /// <param name="context">The processing context for the operation.</param>
        /// <param name="digitalTwin">The digital twin instance receiving the message.</param>
        /// <param name="msgBytes">The raw message data as a byte array.</param>
        /// <returns>A <see cref="ProcessingResult"/> representing the outcome of the message processing.</returns>
        /// <remarks>
        /// <para>
        /// A <see cref="ProcessingResult.NoUpdate"/> value can be returned to
        /// indicate that the <paramref name="digitalTwin"/> instance was not modified 
        /// and does not need to be updated in the ScaleOut service.
        /// <see cref="ProcessingResult.NoUpdate"/> should not be returned if the object
        /// was modified in any way.
        /// If you are unsure of whether the <paramref name="digitalTwin"/> was modified, 
        /// return <see cref="ProcessingResult.DoUpdate"/>.
        /// </remarks>
        public async override Task<ProcessingResult> ProcessMessageAsync(ProcessingContext<SimulatedGasSensorModel> context, SimulatedGasSensorModel digitalTwin, byte[] msgBytes)
        {
            // Deserialize the incoming message:
            BaseMessage? message = System.Text.Json.JsonSerializer.Deserialize<BaseMessage>(msgBytes);
            if (message == null)
            {
                await context.LogMessageAsync(LogSeverity.Warning, string.Format(
                    "Received a message that could not be deserialized for object '{0}'.", digitalTwin.Id));
                return ProcessingResult.NoUpdate;
            }

            switch (message)
            {
                case DeviceCommand controlMessage:
                    if (controlMessage.Code == 100) // set to Alarmed state and make it Inactive on the next simulation step
                        digitalTwin.SensorStatus = SensorStatus.Alarmed;
                    break;
                default:
                    throw new NotImplementedException($"Message processor does not support message type {message.GetType()}");
            }
            return ProcessingResult.DoUpdate;
        }
    }
}
