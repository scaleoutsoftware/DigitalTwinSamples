using Messages;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedWindTurbine
{
    /// <summary>
    /// Handles messages sent from a real-time digital twin. 
    /// </summary>
    /// <remarks>
    /// Messages sent here typically originate from ProcessingContext.SendToDataSource() calls made
    /// in a real-time digital twin's MessageProcessor implementation. The model
    /// in this simulation project is simulating a data source (a real-world device),
    /// so when a real-time model sends a message back to its data source during a simulation run, 
    /// the message arrives here.
    /// </remarks>
    public class SimulatedWindTurbineMessageProcessor : MessageProcessor<SimulatedWindTurbineModel, DigitalTwinMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, SimulatedWindTurbineModel digitalTwin, IEnumerable<DigitalTwinMessage> newMessages)
        {
            //  If your simulation model receives messages, process them for the supplied digital twin.
            /*
            foreach (DigitalTwinMessage message in newMessages)
            {
                switch (message)
                {
                    case ExampleMessage exampleMessage:
                        digitalTwin.SomeState = exampleMessage.StringPayload;
                        break;
                    default:
                        throw new NotImplementedException($"Message processor does not support message type {message.GetType()}");
                }
            }
            */

            // Return ProcessingResult.DoUpdate if this method modified the state of this digital twin instance 
            // to persist the changes back to ScaleOut StreamServer;
            // otherwise, if no changes occurred or the changes are to be discarded,
            // return ProcessingResult.NoUpdate as an optimization to reduce update overhead.
            return ProcessingResult.DoUpdate;
        }
    }
}
