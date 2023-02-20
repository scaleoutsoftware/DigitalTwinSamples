using WindTurbineMessages;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedWindTurbine
{
    public class WindTurbineSimulationMessageProcessor : MessageProcessor<WindTurbineSimulationModel, WindTurbineMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, WindTurbineSimulationModel digitalTwin, IEnumerable<WindTurbineMessage> newMessages)
        {
            throw new NotImplementedException();
        }
    }
}
