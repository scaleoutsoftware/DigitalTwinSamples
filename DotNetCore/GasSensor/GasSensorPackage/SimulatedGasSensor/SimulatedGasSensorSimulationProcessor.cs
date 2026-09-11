using GasSensorPackage.Messages;
using Microsoft.Extensions.Logging.Abstractions;
using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GasSensorPackage.SimulatedGasSensor
{
    public class SimulatedGasSensorSimulationProcessor : SimulationProcessor<SimulatedGasSensorModel>
    {
        ILogger<SimulatedGasSensorSimulationProcessor> _logger;
        const string _targetRealTimeModel = "RealTimeGasSensor";

        public SimulatedGasSensorSimulationProcessor(ILogger<SimulatedGasSensorSimulationProcessor>? logger)
        {
            _logger = logger ?? NullLogger<SimulatedGasSensorSimulationProcessor>.Instance;
        }

        /// <summary>
        /// Method called by the simulation runtime to process a simulation step for a 
        /// simulation instance.
        /// </summary>
        /// <param name="context">The simulation processing context.</param>
        /// <param name="digitalTwin">The simulation instance being processed.</param>
        /// <param name="currentTime">The current simulation time.</param>
        /// <returns>A <see cref="ProcessingResult"/> representing the outcome of the simulation processing.</returns>
        /// <remarks>
        /// <para>
        /// A <see cref="ProcessingResult.NoUpdate"/> value can be returned to
        /// indicate that the <paramref name="digitalTwin"/> instance was not modified 
        /// and does not need to be updated in the ScaleOut service.
        /// <see cref="ProcessingResult.NoUpdate"/> should not be returned if the object
        /// was modified in any way.
        /// If you are unsure of whether the <paramref name="digitalTwin"/> was modified, 
        /// return <see cref="ProcessingResult.DoUpdate"/>.
        /// </remarks>
        public async override Task<ProcessingResult> ProcessModelAsync(ProcessingContext<SimulatedGasSensorModel> context, SimulatedGasSensorModel digitalTwin, DateTimeOffset currentTime)
        {
            ProcessingResult result = ProcessingResult.DoUpdate;
            try
            {
                var simulationController = context.SimulationController;

                digitalTwin.NumberOfSimIterations++;

                if (digitalTwin.SensorStatus == SensorStatus.Alarmed)
                {
                    simulationController.Delay(TimeSpan.FromSeconds(30));
                    await context.LogMessageAsync(LogSeverity.Informational, $"A gas sensor '{digitalTwin.Id}' ({digitalTwin.Site} site) is turned off for 30 seconds.");
                    digitalTwin.SensorStatus = SensorStatus.Inactive;
                }
                else
                {
                    if (digitalTwin.SensorStatus == SensorStatus.Inactive) // after delay is elapsed, make the sensor active again
                    {
                        digitalTwin.SensorStatus = SensorStatus.Active;
                        await context.LogMessageAsync(LogSeverity.Informational, $"A gas sensor '{digitalTwin.Id}' ({digitalTwin.Site} site) is Active again.");
                    }

                    var currentPPMValue = ProducePPMValue(digitalTwin);
                    digitalTwin.CurrentPPMValue = currentPPMValue;

                    BaseMessage telemetry = new GasSensorTelemetry()
                    {
                        PPMReading = currentPPMValue,
                        Timestamp = DateTime.UtcNow
                    };

                    byte[] telemetryBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(telemetry);
                    await simulationController.EmitTelemetryAsync(_targetRealTimeModel, telemetryBytes);
                }

            }
            catch (Exception ex)
            {
                await context.LogMessageAsync(LogSeverity.Error, $"{ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Method called by the simulation runtime to initialize a simulation instance at the start of a simulation run.
        /// </summary>
        /// <param name="context">The simulation initialization context.</param>
        /// <param name="digitalTwin">The simulation instance being initialized.</param>
        /// <param name="startTime">The start time of the simulation run.</param>
        /// <returns>A <see cref="ProcessingResult"/> representing the outcome of the initialization.</returns>
        /// <remarks>
        /// If the same <paramref name="digitalTwin"/> instance is used for multiple simulation runs, 
        /// this method can be used to initialize the instance prior to each run. 
        /// If the instance is only used for a single run, initialization logic 
        /// can be placed in the <see cref="SimulatedGasSensorModel.InitAsync"/> method instead.
        /// </remarks>
        public override ProcessingResult OnInitSimulation(InitSimulationContext context, SimulatedGasSensorModel digitalTwin, DateTimeOffset startTime)
        {
            return base.OnInitSimulation(context, digitalTwin, startTime);
        }

        public int ProducePPMValue(SimulatedGasSensorModel dt)
        {
            int result = 0;

            switch (dt.Site)
            {
                case "Seattle":
                    result = Random.Shared.Next(1, 30);
                    break;
                case "Los Angeles":
                    result = Random.Shared.Next(5, 45);
                    break;
                case "Miami":
                    if (dt.Id == "33035" || dt.Id == "33031" || dt.Id == "33189" || dt.Id == "33193" || dt.Id == "33146")
                        result = Random.Shared.Next(40, 150);
                    else
                        result = Random.Shared.Next(1, 49);
                    break;
                case "Miami Beach":
                    result = Random.Shared.Next(51, 199);
                    break;
                case "New York":
                    if (dt.Id == "11697" || dt.Id == "11229" || dt.Id == "10305")
                    {
                        if (dt.NumberOfSimIterations % 30 == 0)
                        {
                            dt.SensorStatus = SensorStatus.Alarmed;
                            result = 210;
                        }
                        else
                            result = Random.Shared.Next(1, 49);
                    }
                    else
                        result = Random.Shared.Next(20, 40);
                    break;
                case "New Ark":
                    result = Random.Shared.Next(201, 251);
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
