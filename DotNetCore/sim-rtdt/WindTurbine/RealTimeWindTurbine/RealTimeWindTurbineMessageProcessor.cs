using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scaleout.Streaming.DigitalTwin.Core;
using WindTurbineMessages;


namespace RealTimeWindTurbine
{
    public class RealTimeWindTurbineMessageProcessor : MessageProcessor<RealTimeWindTurbineModel, TemperatureReading>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, RealTimeWindTurbineModel digitalTwin, IEnumerable<TemperatureReading> newMessages)
        {
            try
            {
                // Process incoming messages
                foreach (var msg in newMessages)
                {
                    digitalTwin.Temperature = msg.Temperature;
                    if (digitalTwin.Temperature > TemperatureRange.High)
                    {
                        // Overheating. Issue a shutdown command to the device:
                        var cmd = new DeviceCommandMessage { CommandText = "shutdown" };
                        string cmdJson = JsonConvert.SerializeObject(cmd);
                        byte[] cmdBytes = Encoding.UTF8.GetBytes(cmdJson);
                        context.SendToDataSource(cmdBytes);
                    }
                }

                // Use the embedded logger to log messages.
                context.LogMessage(LogSeverity.Informational,
                       string.Format("The real-time digital twin object '{0}' has successfully processed {1} message(s)",
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
