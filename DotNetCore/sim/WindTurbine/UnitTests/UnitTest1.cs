using System;
using Xunit;
using Newtonsoft.Json;
using Scaleout.DigitalTwin.Mock;
using SimulatedWindTurbine;
using WindTurbineMessages;


namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MockEnvironment mockEnv = new MockEnvironment();
            MockEndpoint mockEndpoint = mockEnv.CreateMockEndpoint(modelName: "WindTurbine",
                                                                   messageProcessor: new WindTurbineSimulationMessageProcessor(),
                                                                   alertProviders: null,
                                                                   logger: null);

            // Prepare message:
            WindTurbineMessage msg = new WindTurbineMessage
            {
                Timestamp = DateTime.Now
            };
            string msgJson = JsonConvert.SerializeObject(msg);

            // Send the message:
            mockEndpoint.Send("Simulated Instance 001", msgJson);

            // Inspect the model instance, confirm the message was processed:
            var instance = mockEndpoint.GetInstances()["WindTurbine Instance 001"] as WindTurbineSimulationModel;
            Assert.True(instance.MessageList.Count == 1);
        }
    }
}