using System;
using Messages;
using Scaleout.Streaming.DigitalTwin.Core;

namespace SimulatedWindTurbine
{
    public class SimulatedWindTurbineProcessor : SimulationProcessor<SimulatedWindTurbineModel>
    {
        public override ProcessingResult ProcessModel(ProcessingContext context, 
                                                      SimulatedWindTurbineModel simTurbine, 
                                                      DateTimeOffset currentTime)
        {
            simTurbine.CurrentRPMs++;
            simTurbine.CurrentTemp++;

            var msg = new WindTurbineMessage
            {
                RPM = simTurbine.CurrentRPMs,
                Temperature = simTurbine.CurrentTemp,
                Timestamp = context.GetCurrentTime()
            };
            context.SimulationController.EmitTelemetry("RealTimeWindTurbine", msg);

            return ProcessingResult.DoUpdate;
        }
    }
}
