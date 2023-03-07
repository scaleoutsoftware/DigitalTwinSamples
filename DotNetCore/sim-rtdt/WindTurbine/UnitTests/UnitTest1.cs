using System;
using Xunit;
using Newtonsoft.Json;
using Scaleout.DigitalTwin.Mock;
using RealTimeWindTurbine;
using WindTurbineMessages;
using Scaleout.Client;
using Scaleout.DigitalTwin.Client;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MockEnvironment mockEnv = new MockEnvironment();
            MockEndpoint mockEndpoint = mockEnv.CreateMockEndpoint(modelName: "RealTimeWindTurbine",
                                                                   messageProcessor: new RealTimeWindTurbineMessageProcessor(),
                                                                   alertProviders: null,
                                                                   logger: null);

            // Prepare message:
            TemperatureReading msg = new TemperatureReading
            {
                Temperature = 33
            };
            string msgJson = JsonConvert.SerializeObject(msg);

            // Send the message:
            mockEndpoint.Send("WindTurbine Instance 001", msgJson);

            // Inspect the model instance, confirm the message was processed:
            var instance = mockEndpoint.GetInstances()["WindTurbine Instance 001"] as RealTimeWindTurbineModel;
        }

        [Fact]
        public void CreateSimulationInstance()
        {
            // Connect to the ScaleOut service and create an endpoint to the 
            // WindTurbineSim digital twin model.
            var conn = GridConnection.Connect("hosts=localhost:721");
            var dtEndpoint = new DigitalTwinModelEndpoint(conn, "SimulatedWindTurbine");

            var simTwin = new SimulatedWindTurbine.WindTurbineSimulationModel
            {
                Temperature = 60,
                FailureRate = 100
            };
            dtEndpoint.CreateTwin($"simeTwin1", simTwin);

        }
    }
}
