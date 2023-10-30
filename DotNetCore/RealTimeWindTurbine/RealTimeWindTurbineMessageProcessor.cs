using Messages;
using Newtonsoft.Json;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeWindTurbine
{
    public class RealTimeWindTurbineMessageProcessor : 
                 MessageProcessor<RealTimeWindTurbineModel, DigitalTwinMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, 
                                                         RealTimeWindTurbineModel digitalTwin, 
                                                         IEnumerable<DigitalTwinMessage> newMessages)
        {
            try
            {
                // Process incoming messages
                foreach (var message in newMessages)
                {
                    switch (message)
                    {
                        case WindTurbineMessage windTurbineMsg:
                            // Store the incoming message for later historical analysis
                            // in the model instance.
                            digitalTwin.GearboxTemps.Add(windTurbineMsg.Temperature);
                            digitalTwin.RPMs.Add(windTurbineMsg.RPM);
                            break;
                        default:
                            throw new NotImplementedException($"Message processor does not support message type {message.GetType()}");
                    }

                    // We can optionally send a message or alert back to a data source (e.g. IoT device):
                    if (digitalTwin.GearboxTemps.Last() > 120.0)
                    {
                        var msg = new { Command = "Shutdown" };
                        context.SendToDataSource(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                context.LogMessage(LogSeverity.Error, string.Format(
                    "Exception occurred while processing new messages for object '{0}'. Details: {1}",
                    digitalTwin.Id, ex.Message));
            }

            // Persist changes to the digitalTwin.
            return ProcessingResult.DoUpdate;
        }
    }
}
