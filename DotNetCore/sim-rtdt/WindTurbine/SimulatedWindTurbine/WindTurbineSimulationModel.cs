using System;
using System.Collections.Generic;

using Scaleout.Streaming.DigitalTwin.Core;
using WindTurbineMessages;

namespace SimulatedWindTurbine
{
    public enum TurbineState
    {
        Running,
        Overheating,
        Idle
    }

    public class WindTurbineSimulationModel : DigitalTwinBase
    {
        /// <summary>
        /// Get's the simulated temperature at this time interval. Call
        /// <see cref="AdvanceToNextState"/> to move this instance to the next time
        /// interval.
        /// </summary>
        public int Temperature { get; set; } = TemperatureRange.NormalOperationMin;

        public TurbineState State { get; set; } = TurbineState.Running;

        /// <summary>
        /// Sets the likelihood of an overheat failure. Higher numbers correspond to 
        /// a lower likelihood: Setting to 10,000 indicates that the instance will
        /// fail once out of every 10,000 time interval samples.
        /// </summary>
        public int FailureRate { get; set; }

        private Random _rand = new Random(Guid.NewGuid().GetHashCode());
        
        /// <summary>
        /// Moves that state of the simulation instance to its next time slice.
        /// </summary>
        public void AdvanceToNextState()
        {
            switch (State)
            {
                case TurbineState.Running:
                    if (_rand.Next(FailureRate) == 0)
                    {
                        // Transition to Overheating state.
                        State = TurbineState.Overheating;
                        Temperature = _rand.Next(TemperatureRange.NormalOperationMax, TemperatureRange.OverheatMax);
                    }
                    else
                    {
                        Temperature = _rand.Next(TemperatureRange.NormalOperationMin, TemperatureRange.NormalOperationMax);
                    }
                    break;
                case TurbineState.Overheating:
                    // Continue overheating.
                    Temperature = _rand.Next(TemperatureRange.NormalOperationMax, TemperatureRange.OverheatMax);
                    break;
                case TurbineState.Idle:
                    Temperature = TemperatureRange.Idle;
                    break;
                default:
                    throw new NotSupportedException($"Unexpected turbine state {State}.");
            }
        }
    }
}
