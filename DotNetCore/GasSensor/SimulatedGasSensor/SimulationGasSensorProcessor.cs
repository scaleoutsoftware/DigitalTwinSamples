/* 
 * © Copyright 2024 by ScaleOut Software, Inc.
 *
 * LICENSE AND DISCLAIMER
 * ----------------------
 * This material contains sample programming source code ("Sample Code").
 * ScaleOut Software, Inc. (SSI) grants you a nonexclusive license to compile, 
 * link, run, display, reproduce, and prepare derivative works of 
 * this Sample Code.  The Sample Code has not been thoroughly
 * tested under all conditions.  SSI, therefore, does not guarantee
 * or imply its reliability, serviceability, or function. SSI
 * provides no support services for the Sample Code.
 *
 * All Sample Code contained herein is provided to you "AS IS" without
 * any warranties of any kind. THE IMPLIED WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGMENT ARE EXPRESSLY
 * DISCLAIMED.  SOME JURISDICTIONS DO NOT ALLOW THE EXCLUSION OF IMPLIED
 * WARRANTIES, SO THE ABOVE EXCLUSIONS MAY NOT APPLY TO YOU.  IN NO 
 * EVENT WILL SSI BE LIABLE TO ANY PARTY FOR ANY DIRECT, INDIRECT, 
 * SPECIAL OR OTHER CONSEQUENTIAL DAMAGES FOR ANY USE OF THE SAMPLE CODE
 * INCLUDING, WITHOUT LIMITATION, ANY LOST PROFITS, BUSINESS 
 * INTERRUPTION, LOSS OF PROGRAMS OR OTHER DATA ON YOUR INFORMATION
 * HANDLING SYSTEM OR OTHERWISE, EVEN IF WE ARE EXPRESSLY ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGES.
 */
using System;
using System.IO;

using Scaleout.Streaming.DigitalTwin.Core;
using Scaleout.Streaming.DigitalTwin.Core.Exceptions;
using ScaleOut.DigitalTwin.Samples.GasSensor.Messages;

namespace ScaleOut.DigitalTwin.Samples.SimulatedGasSensor
{
    /// <summary>
    /// Simulation support: Your implementation of a simulation processor that gets triggered 
    /// for every time interval in a simulation.
    /// </summary>
    public class SimulationGasSensorProcessor : SimulationProcessor<SimulatedGasSensorModel>
    {
        static object _lock = new object();
        const string _targetRealTimeModel = "GasSensorTwin";

        /// <summary>
        /// This method is called by ScaleOut Digital Twins every time when the next simulation time 
        /// interval is elapsed.
        /// </summary>
        /// <param name="context">The digital twin simulation processing context.</param>
        /// <param name="digitalTwin">The target digital twin object.</param>
        /// <param name="currentTime">The current simulation time.</param>
        /// <returns>
        /// <see cref="ProcessingResult.DoUpdate"/> when the digital twin object needs to be updated, and 
        /// <see cref="ProcessingResult.NoUpdate"/> when no updates are needed.
        /// </returns>
        public override ProcessingResult ProcessModel(ProcessingContext context, SimulatedGasSensorModel digitalTwin, DateTimeOffset currentTime)
        {
            ProcessingResult result = ProcessingResult.DoUpdate;
            try
            {
                var simulationController = context.SimulationController;
                if (simulationController != null)
                {
                    digitalTwin.NumberOfSimIterations++;

                    if (digitalTwin.SensorStatus == SensorStatus.Alarmed)
                    {
                        simulationController.Delay(TimeSpan.FromSeconds(30));
                        WriteLogMessage(context, LogSeverity.Informational, message: $"A gas sensor '{digitalTwin.Id}' ({digitalTwin.Site} site) is turned off for 30 seconds.");
                        digitalTwin.SensorStatus = SensorStatus.Inactive;
                    }
                    else
                    {
                        if (digitalTwin.SensorStatus == SensorStatus.Inactive) // after delay is elapsed, make the sensor active again
                        {
                            digitalTwin.SensorStatus = SensorStatus.Active;
                            WriteLogMessage(context, LogSeverity.Informational, message: $"A gas sensor '{digitalTwin.Id}' ({digitalTwin.Site} site) is Active again.");
                        }

                        var currentPPMValue = ProducePPMValue(digitalTwin);
                        digitalTwin.CurrentPPMValue = currentPPMValue;

                        var telemetry = new GasSensorTelemetry()
                        {
                            PPMReading = currentPPMValue,
                            Timestamp = DateTime.UtcNow
                        };

                        SendingResult res = simulationController.EmitTelemetry(_targetRealTimeModel, telemetry);
                        if (res != SendingResult.Handled)
                            WriteLogMessage(context: context, LogSeverity.Error, message: "Weak connection - failed to send telemetry.");
                    }
                }
                else
                    WriteLogMessage(context: null, LogSeverity.Error, message: "IModelSimulation is null");

            }
            catch (ModelSimulationException mse)
            {
                WriteLogMessage(context, LogSeverity.Error, $"{mse.Message}");
            }

            return result;
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

        private void WriteLogMessage(ProcessingContext context, LogSeverity severity, string message)
        {
            string logDirectory = @"C:\Temp";

            // Create the target directory if it does not exist
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            lock (_lock)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(logDirectory, "SimulatedGasSensor.log"), true))
                {
                    file.WriteLine($"[{DateTime.Now.ToString("G")}]  {message}");
                }
            }
            // Log this message to the log object
            if (context != null)
                context.LogMessage(severity, message);
        }
    }
}
