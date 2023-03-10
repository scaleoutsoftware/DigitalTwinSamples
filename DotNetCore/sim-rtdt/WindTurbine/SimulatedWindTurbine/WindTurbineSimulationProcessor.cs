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
    /// <summary>
    /// Simulation processor that gets triggered for every time interval in a simulation.
    /// </summary>
    public class WindTurbineSimulationProcessor : SimulationProcessor<WindTurbineSimulationModel>
    {
        /// <summary>
        /// This method is called by the service when the next simulation 
        /// interval has elapsed. Use it to update a simulation's state to reflect the
        /// given time interval and send any desired telemetry to the real-time digital twin.
        /// </summary>
        /// <param name="context">The digital twin simulation processing context.</param>
        /// <param name="digitalTwin">The target digital twin object.</param>
        /// <param name="currentTime">The current simulation time.</param>
        /// <returns>
        /// <see cref="ProcessingResult.DoUpdate"/> when the digital twin object needs to be updated, and 
        /// <see cref="ProcessingResult.NoUpdate"/> when no updates are needed.
        /// </returns>
        public override ProcessingResult ProcessModel(ProcessingContext context, WindTurbineSimulationModel digitalTwin, DateTimeOffset currentTime)
        {
            
            digitalTwin.AdvanceToNextState();
            var msg = new TemperatureReading
            {
                Temperature = digitalTwin.Temperature,
                TimestampUtc = currentTime.UtcDateTime
            };
            var msgJson = JsonConvert.SerializeObject(msg);
            var msgBytes = Encoding.UTF8.GetBytes(msgJson);

            context.SimulationController.EmitTelemetry("RealTimeWindTurbine", msgBytes);

            // The digitalTwin.AdvanceToNextState() call modified the instance,
            // so we return DoUpdate to ensure that the simulation instance
            // is updated in the ScaleOut service.
            return ProcessingResult.DoUpdate;
        }
    }
}
