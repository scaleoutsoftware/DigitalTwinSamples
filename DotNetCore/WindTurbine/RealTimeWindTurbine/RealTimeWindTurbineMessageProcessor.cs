using Microsoft.Extensions.Logging.Abstractions;
using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeWindTurbine
{
    public class RealTimeWindTurbineMessageProcessor : MessageProcessor<RealTimeWindTurbineModel>
    {
        ILogger<RealTimeWindTurbineMessageProcessor> _logger;

        /// <summary>
        /// Constructor, arguments supplied by DI.
        /// </summary>
        public RealTimeWindTurbineMessageProcessor(ILogger<RealTimeWindTurbineMessageProcessor>? logger)
        {
            _logger = logger ?? NullLogger<RealTimeWindTurbineMessageProcessor>.Instance;
        }

        /// <summary>
        /// Processes a message sent to the specified digital twin instance.
        /// </summary>
        public async override Task<ProcessingResult> ProcessMessageAsync(
                                   ProcessingContext<RealTimeWindTurbineModel> context,
                                   RealTimeWindTurbineModel digitalTwin,
                                   byte[] msgBytes)
        {
            try
            {
                // Deserialize the incoming message (e.g. from an IoT device):
                WindTurbineMessage? message = System.Text.Json.JsonSerializer.Deserialize<WindTurbineMessage>(msgBytes);
                if (message == null)
                {
                    // LogMessageAsync displays messages in the ScaleOut Digital Twins UI.
                    await context.LogMessageAsync(LogSeverity.Warning, string.Format(
                        "Received a message that could not be deserialized for object '{0}'.", digitalTwin.Id));
                    return ProcessingResult.NoUpdate;
                }

                // Update the digital twin's state based on the message:
                digitalTwin.GearboxTemps.Add(message.Temperature);
                digitalTwin.RPMs.Add(message.RPM);

                // We can optionally send a message or alert back to a data source (e.g. IoT device):
                if (digitalTwin.GearboxTemps.Last() > 120.0)
                {
                    var msg = new { Command = "Shutdown" };
                    byte[] msgBytesOut = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
                    await context.SendToDataSourceAsync(msgBytesOut);
                }
            }
            catch (Exception ex)
            {
                await context.LogMessageAsync(LogSeverity.Error, string.Format(
                    "Exception occurred while processing new messages for object '{0}'. Details: {1}",
                    digitalTwin.Id, ex.Message));
            }

            // Persist modification made to the digitalTwin's state.
            return ProcessingResult.DoUpdate;
        }
    }
}
