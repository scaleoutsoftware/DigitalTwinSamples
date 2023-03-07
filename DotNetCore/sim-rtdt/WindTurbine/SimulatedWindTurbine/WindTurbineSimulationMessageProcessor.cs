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
    public class WindTurbineSimulationMessageProcessor : MessageProcessor<WindTurbineSimulationModel, DeviceCommandMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, WindTurbineSimulationModel digitalTwin, IEnumerable<DeviceCommandMessage> newMessages)
        {
            try
            {
                // Process incoming messages
                foreach (var msg in newMessages)
                {
                    switch (msg.CommandText.ToLower())
                    {
                        case "shutdown":
                            digitalTwin.State = TurbineState.Idle;
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
                // If the ScaleOut Digital Twin Streaming Service is used to host real-time digital twins, 
                // you can see both trace messages and logged exceptions on the Manage Digital Twin Model page.
                context.LogMessage(LogSeverity.Error,
                       string.Format("Exception occurred while processing new messages for the real-time digital twin object '{0}'. Details: {1}",
                       digitalTwin.Id, ex.Message));
            }

            // Return ProcessingResult.DoUpdate since this method modified the state of the digitalTwin instance.
            return ProcessingResult.DoUpdate;
        }
    }
}
