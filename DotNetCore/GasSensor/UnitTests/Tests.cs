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
using ScaleOut.DigitalTwin.Samples.GasSensorTwin;
using ScaleOut.DigitalTwin.Samples.SimulatedGasSensor;
using ScaleOut.DigitalTwin.Samples.GasSensor.Messages;

namespace ScaleOut.DigitalTwin.Samples.GasSensor.UnitTests
{
    public class Tests
    {
        [Fact]
        public void TestSimulation()
        {
            SimulationWorkbench wb = new SimulationWorkbench();
            wb.AddSimulationModel("SimulatedGasSensor", new SimulationGasSensorProcessor(), new SimulatedGasSensorMessageProcessor());
            wb.AddRealTimeModel("GasSensorTwin", new GasSensorTwinMessageProcessor());

            var gasSimSensor1 = new SimulatedGasSensorModel { Site = "Seattle" };
            var gasSimSensor2 = new SimulatedGasSensorModel { Site = "Los Angeles" };
            var gasSimSensor3 = new SimulatedGasSensorModel { Site = "Miami" };
            var gasSimSensor4 = new SimulatedGasSensorModel { Site = "New York" };
            wb.AddInstance("Sensor1", "SimulatedGasSensor", gasSimSensor1);
            wb.AddInstance("Sensor2", "SimulatedGasSensor", gasSimSensor2);
            wb.AddInstance("Sensor3", "SimulatedGasSensor", gasSimSensor3);
            wb.AddInstance("Sensor4", "SimulatedGasSensor", gasSimSensor4);

            var gasRTSensor1 = new GasSensorTwinModel { Site = "Seattle" };
            var gasRTSensor2 = new GasSensorTwinModel { Site = "Los Angeles" };
            var gasRTSensor3 = new GasSensorTwinModel { Site = "Miami" };
            var gasRTSensor4 = new GasSensorTwinModel { Site = "New York" };
            wb.AddInstance("Sensor1", "GasSensorTwin", gasRTSensor1);
            wb.AddInstance("Sensor2", "GasSensorTwin", gasRTSensor2);
            wb.AddInstance("Sensor3", "GasSensorTwin", gasRTSensor3);
            wb.AddInstance("Sensor4", "GasSensorTwin", gasRTSensor4);

            DateTime startTime = new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 0);
            //wb.InitializeSimulation(startTime, endTime: new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 1, second: 0), simulationIterationInterval: TimeSpan.FromSeconds(1));
            //StepResult stepResult;
            //do
            //{
            //    stepResult = wb.Step();
            //} while (stepResult.SimulationStatus == SimulationStatus.Running);

            wb.RunSimulation(startTime, endTime: new DateTime(year: 2023, month: 1, day: 1, hour: 0, minute: 0, second: 50), simulationIterationInterval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            var simInstances = wb.GetInstances<SimulatedGasSensorModel>("SimulatedGasSensor");
            Assert.True(simInstances["Sensor3"].SensorStatus == SensorStatus.Inactive);
            Assert.True(simInstances["Sensor4"].SensorStatus == SensorStatus.Inactive);
        }

        [Fact]
        public void NoAlarm()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel(modelName: "GasSensorTwin", processor: new GasSensorTwinMessageProcessor());

            int ppmValue = 10;
            var msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            endpoint.Send("Sensor1", msg);

            ppmValue = 20;
            msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            endpoint.Send("Sensor1", msg);

            var rtInstances = wb.GetInstances<GasSensorTwinModel>(modelName: "GasSensorTwin");
            foreach(var rtTwin in rtInstances.Values)
                Assert.True(rtTwin.AlarmSounded == 0);
        }

        [Fact]
        public void AlarmDueToPeekValue()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel(modelName: "GasSensorTwin", processor: new GasSensorTwinMessageProcessor());

            int ppmValue = 10;
            var msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            endpoint.Send("Sensor1", msg);

            ppmValue = 20;
            msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            endpoint.Send("Sensor1", msg);

            ppmValue = 250;
            msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
            endpoint.Send("Sensor1", msg);

            var rtInstances = wb.GetInstances<GasSensorTwinModel>(modelName: "GasSensorTwin");
            foreach (var rtTwin in rtInstances.Values)
                Assert.True(rtTwin.AlarmSounded == 1);
        }

        [Fact]
        public void AlarmDueToHighLevelOverTime()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel(modelName: "GasSensorTwin", processor: new GasSensorTwinMessageProcessor());
            int ppmValue = 0;

            for (int i = 0; i < GasSensorTwinModel.MaxAllowedTimePeriod.TotalSeconds + 5; i++)
            {
                ppmValue = Random.Shared.Next(GasSensorTwinModel.MaxAllowedPPM, GasSensorTwinModel.MaxAllowedPPM + 100);

                var msg = new GasSensorTelemetry { PPMReading = ppmValue, Timestamp = DateTime.UtcNow };
                endpoint.Send("Sensor1", msg);

                Thread.Sleep(1000); // sleep for 1 second
            }

            var rtInstances = wb.GetInstances<GasSensorTwinModel>(modelName: "GasSensorTwin");
            foreach (var rtTwin in rtInstances.Values)
                Assert.True(rtTwin.AlarmSounded == 1);
        }
    }
}