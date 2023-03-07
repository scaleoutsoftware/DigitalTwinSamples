using Newtonsoft.Json;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindTurbineMessages;

namespace SimulatedWindTurbine
{
    public class WindTurbineSimulationProcessor : SimulationProcessor<WindTurbineSimulationModel>
    {
        public override ProcessingResult ProcessModel(ProcessingContext context, WindTurbineSimulationModel digitalTwin, DateTimeOffset currentTime)
        {
            
            digitalTwin.NextState();
            var msg = new TemperatureReading
            {
                Temperature = digitalTwin.Temperature,
                TimestampUtc = currentTime.UtcDateTime
            };
            var msgJson = JsonConvert.SerializeObject(msg);
            var msgBytes = Encoding.UTF8.GetBytes(msgJson);

            context.SimulationController.EmitTelemetry("RealTimeWindTurbine", msgBytes);

            // The digitalTwin.NextState() call modified the instance,
            // so we return DoUpdate to ensure that the simulation instance
            // is updated in the ScaleOut service.
            return ProcessingResult.DoUpdate;
        }
    }
}
