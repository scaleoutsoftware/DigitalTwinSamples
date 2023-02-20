using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Scaleout.Streaming.DigitalTwin.Core;
using WindTurbineMessages;


namespace RealTimeWindTurbine
{
    public class RealTimeWindTurbineMessageProcessor : MessageProcessor<RealTimeWindTurbineModel, WindTurbineMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, RealTimeWindTurbineModel digitalTwin, IEnumerable<WindTurbineMessage> newMessages)
        {
            try
            {
                // Process incoming messages
                foreach (var msg in newMessages)
                {
                    /// ...
                    /// Add context-aware processing logic that makes use of the state object here.
                    /// ...

                    // We can optionally send a message or alert back to a data source (e.g. IoT device):
                    // context.SendToDataSource(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(alert)));

                    // Store the incoming message for later historical analysis in the model instance.
                    digitalTwin.MessageList.Add(msg);
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

            // Return ProcessingResult.DoUpdate if this method modified the state of this digital twin instance 
            // to persist the changes back to ScaleOut StreamServer;
            // otherwise, if no changes occurred or the changes are to be discarded, return ProcessingResult.NoUpdate.
            return ProcessingResult.DoUpdate;
        }
    }
}
