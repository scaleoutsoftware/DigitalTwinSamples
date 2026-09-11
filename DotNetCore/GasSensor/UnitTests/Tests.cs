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
using Scaleout.DigitalTwin.Workbench;
using GasSensorPackage;
using GasSensorPackage.SimulatedGasSensor;
using GasSensorPackage.RealTimeGasSensor;
using GasSensorPackage.Messages;

namespace ScaleOut.DigitalTwin.Samples.GasSensor.UnitTests
{
    public class Tests
    {
        [Fact]
        public async Task TestSimulation()
        {
            SimulationWorkbench wb = new SimulationWorkbench();
            wb.AddSimulationModel(
                "SimulatedGasSensor", 
                new SimulatedGasSensorSimulationProcessor(null), 
                new SimulatedGasSensorMessageProcessor(null));

            wb.AddRealTimeModel(
                "RealTimeGasSensor", 
                new RealTimeGasSensorMessageProcessor(null));

            var gasSimSensor1 = new SimulatedGasSensorModel { Site = "Seattle" };
            var gasSimSensor2 = new SimulatedGasSensorModel { Site = "Los Angeles" };
            var gasSimSensor3 = new SimulatedGasSensorModel { Site = "Miami Beach" }; // It's guaranteed to trip the threshold
            var gasSimSensor4 = new SimulatedGasSensorModel { Site = "New Ark" };     // It's guaranteed to trip the threshold
            wb.AddInstance("Sensor1", "SimulatedGasSensor", gasSimSensor1);
            wb.AddInstance("Sensor2", "SimulatedGasSensor", gasSimSensor2);
            wb.AddInstance("Sensor3", "SimulatedGasSensor", gasSimSensor3);
            wb.AddInstance("Sensor4", "SimulatedGasSensor", gasSimSensor4);

            var gasRTSensor1 = new RealTimeGasSensorModel { Site = "Seattle" };
            var gasRTSensor2 = new RealTimeGasSensorModel { Site = "Los Angeles" };
            var gasRTSensor3 = new RealTimeGasSensorModel { Site = "Miami Beach" };
            var gasRTSensor4 = new RealTimeGasSensorModel { Site = "New Ark" };
            wb.AddInstance("Sensor1", "RealTimeGasSensor", gasRTSensor1);
            wb.AddInstance("Sensor2", "RealTimeGasSensor", gasRTSensor2);
            wb.AddInstance("Sensor3", "RealTimeGasSensor", gasRTSensor3);
            wb.AddInstance("Sensor4", "RealTimeGasSensor", gasRTSensor4);

            DateTime startTime = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0);
            //wb.InitializeSimulation(startTime, endTime: new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 1, second: 0), simulationIterationInterval: TimeSpan.FromSeconds(1));
            //StepResult stepResult;
            //do
            //{
            //    stepResult = wb.Step();
            //} while (stepResult.SimulationStatus == SimulationStatus.Running);

            await wb.RunSimulationAsync(startTime, endTime: new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 35), simulationIterationInterval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            var simInstances = wb.GetInstances<SimulatedGasSensorModel>("SimulatedGasSensor");
            Assert.True(simInstances["Sensor3"].SensorStatus == SensorStatus.Inactive);
            Assert.True(simInstances["Sensor4"].SensorStatus == SensorStatus.Inactive);
        }

        [Fact]
        public async Task NoAlarm()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel(modelName: "RealTimeGasSensor", new RealTimeGasSensorMessageProcessor(null));

            int ppmValue = 10;
            BaseMessage msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            byte[] msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
            await endpoint.SendAsync("Sensor1", msgBytes);

            ppmValue = 20;
            msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
            await endpoint.SendAsync("Sensor1", msgBytes);

            var rtInstances = wb.GetInstances<RealTimeGasSensorModel>(modelName: "RealTimeGasSensor");
            foreach(var rtTwin in rtInstances.Values)
                Assert.True(rtTwin.AlarmSounded == 0);
        }

        [Fact]
        public async Task AlarmDueToPeakValue()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel(modelName: "RealTimeGasSensor", processor: new RealTimeGasSensorMessageProcessor(null));

            int ppmValue = 10;
            BaseMessage msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            byte[] msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
            await endpoint.SendAsync("Sensor1", msgBytes);

            ppmValue = 20;
            msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
            await endpoint.SendAsync("Sensor1", msgBytes);

            ppmValue = 250;
            msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
            await endpoint.SendAsync("Sensor1", msgBytes);

            var rtInstances = wb.GetInstances<RealTimeGasSensorModel>(modelName: "RealTimeGasSensor");
            foreach (var rtTwin in rtInstances.Values)
                Assert.True(rtTwin.AlarmSounded == 1);
        }

        [Fact]
        public async Task AlarmDueToHighLevelOverTime()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel(modelName: "RealTimeGasSensor", processor: new RealTimeGasSensorMessageProcessor(null));
            int ppmValue = 0;

            for (int i = 0; i < RealTimeGasSensorModel.MaxAllowedTimePeriod.TotalSeconds + 5; i++)
            {
                ppmValue = RealTimeGasSensorModel.MaxAllowedPPM + 100;

                BaseMessage msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
                byte[] msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
                await endpoint.SendAsync("Sensor1", msgBytes);

                await Task.Delay(1000); // sleep for 1 second
            }

            var rtInstances = wb.GetInstances<RealTimeGasSensorModel>(modelName: "RealTimeGasSensor");
            foreach (var rtTwin in rtInstances.Values)
                Assert.True(rtTwin.AlarmSounded == 1);
        }
    }
}