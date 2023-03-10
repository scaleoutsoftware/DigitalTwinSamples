using WindTurbineMessages;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SimulatedWindTurbine
{
    /// <summary>
    /// Handles messages sent from the real-time digital twin. 
    /// </summary>
    /// <remarks>
    /// Messages sent here originate from ProcessingContext.SendToDataSource() calls made
    /// in the RealTimeWindTurbineMessageProcessor. The WindTurbineSimulationModel
    /// in this SimulatedWindTurbine project is simulating a data source (a real-world device),
    /// so when a real-time model sends a message back to its data source during a simulation run, 
    /// the message arrives here.
    /// </remarks>
    public class WindTurbineSimulationMessageProcessor : MessageProcessor<WindTurbineSimulationModel, DeviceCommandMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, WindTurbineSimulationModel digitalTwin, IEnumerable<DeviceCommandMessage> newMessages)
        {
            ProcessingResult result = ProcessingResult.NoUpdate;

            try
            {
                // Process incoming messages
                foreach (var msg in newMessages)
                {
                    switch (msg.CommandText.ToLower())
                    {
                        case "shutdown":
                            digitalTwin.State = TurbineState.Idle;
                            // We've modified the digital twin instance, so return DoUpdate to save its state in the 
                            // ScaleOut service.
                            result = ProcessingResult.DoUpdate;
                            break;
                        case "start":
                            digitalTwin.State = TurbineState.Running;
                            result = ProcessingResult.DoUpdate;
                            break;
                        default:
                            context.LogMessage(LogSeverity.Error,
                                   $"Unknown command '{msg.CommandText}' sent to real-time digital twin object '{digitalTwin.Id}'");
                            break;
                    }
                }

                // Use the embedded logger to log messages.
                context.LogMessage(LogSeverity.Informational,
                       string.Format("The real-time digital twin object '{0}' has processed {1} message(s)",
                       digitalTwin.Id, newMessages.Count()));
            }
            catch (Exception ex)
            {
                // Catch all exceptions and log them using the embedded logger.
                context.LogMessage(LogSeverity.Error,
                       string.Format("Exception occurred while processing new messages for the real-time digital twin object '{0}'. Details: {1}",
                       digitalTwin.Id, ex.Message));
            }

            return result;
        }
    }
}
