using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedWindTurbine
{
    public class WindTurbineSimulationProcessor : SimulationProcessor<WindTurbineSimulationModel>
    {
        public override ProcessingResult ProcessModel(ProcessingContext context, WindTurbineSimulationModel digitalTwin, DateTimeOffset currentTime)
        {
            throw new NotImplementedException();
        }
    }
}
