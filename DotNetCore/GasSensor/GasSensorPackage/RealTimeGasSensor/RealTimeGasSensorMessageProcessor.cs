using GasSensorPackage.Messages;
using Microsoft.Extensions.Logging.Abstractions;
using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasSensorPackage.RealTimeGasSensor
{
    /// <summary>
    /// Handles messages sent to a real-time digital twin instance.
    /// </summary>
    public class RealTimeGasSensorMessageProcessor : MessageProcessor<RealTimeGasSensorModel>
    {
        ILogger<RealTimeGasSensorMessageProcessor> _logger;

        /// <summary>
        /// Message handler constructor. Parameters are supplied via Dependency Injection
        /// and can be modified as needed.
        /// </summary>
        /// <param name="logger">ILogger instance.</param>
        public RealTimeGasSensorMessageProcessor(ILogger<RealTimeGasSensorMessageProcessor>? logger)
        {
            _logger = logger ?? NullLogger<RealTimeGasSensorMessageProcessor>.Instance;
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
        public async override Task<ProcessingResult> ProcessMessageAsync(ProcessingContext<RealTimeGasSensorModel> context, RealTimeGasSensorModel dt, byte[] msgBytes)
        {
            try
            {
                // Deserialize the incoming message:
                BaseMessage? message = System.Text.Json.JsonSerializer.Deserialize<BaseMessage>(msgBytes);
                if (message == null)
                {
                    await context.LogMessageAsync(LogSeverity.Warning, string.Format(
                        "Received a message that could not be deserialized for object '{0}'.", dt.Id));
                    return ProcessingResult.NoUpdate;
                }

                // Vary message processing based on the actual message type:
                switch (message)
                {
                    case GasSensorTelemetry telemetry:
                        dt.LastPPMReading = telemetry.PPMReading;
                        dt.LastPPMTime = telemetry.Timestamp;

                        if (telemetry.PPMReading > RealTimeGasSensorModel.MaxAllowedPPM)
                        {
                            if (!dt.LimitExceeded)
                            {
                                dt.LimitExceeded = true;
                                dt.LimitStartTime = dt.LastPPMTime;
                            }

                            if ((dt.LastPPMTime - dt.LimitStartTime) > RealTimeGasSensorModel.MaxAllowedTimePeriod ||
                                 telemetry.PPMReading >= RealTimeGasSensorModel.SpikeAlertPPM)
                            {
                                dt.AlarmSounded = 1; // notify personnel
                                await context.LogMessageAsync(LogSeverity.Informational, $"The real-time digital twin '{dt.Id}' ({dt.Site} site) has switched to the Alarmed state.");

                                // CODE for BONUS TASK:
                                BaseMessage action = new DeviceCommand()
                                { 
                                    Description = "Shutdown the incoming gas pipe",
                                    Code = 100 // e.g. the command code to shut down a sensor
                                }; 
                                
                                // Send the shutdown gas pipe command back to the device
                                byte[] actionBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(action);
                                await context.SendToDataSourceAsync(actionBytes);
                            }
                        }
                        else
                        {
                            if (dt.AlarmSounded == 1)
                                await context.LogMessageAsync(LogSeverity.Informational, $"The real-time digital twin '{dt.Id}' ({dt.Site} site) has restorated its Active state.");

                            dt.AlarmSounded = 0;
                            dt.LimitExceeded = false;
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Message processor does not support message type {message.GetType()}");
                }
            }
            catch (Exception ex)
            {
                // Catch all exceptions and log them using the embedded logger.
                await context.LogMessageAsync(LogSeverity.Error,
                                    string.Format("Exception occurred while processing new messages for the real-time digital twin object '{0}'. Details: {1}",
                                    dt.Id, ex.Message));
            }

            // Return ProcessingResult.DoUpdate if this method modified the state of this digital twin instance 
            // to persist the changes back to ScaleOut StreamServer;
            // otherwise, if no changes occurred or the changes are to be discarded, return ProcessingResult.NoUpdate.
            return ProcessingResult.DoUpdate;
        }
    }
}
