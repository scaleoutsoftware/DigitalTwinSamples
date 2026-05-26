using RealTimeWindTurbine;
using Scaleout.DigitalTwin.Workbench;

namespace UnitTests
{
    public class RealTimeTests
    {
        [Fact]
        public void SendMessage()
        {
            using RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel("RealTimeWindTurbine", new RealTimeWindTurbineMessageProcessor(null));

            var msg = new WindTurbineMessage
            {
                RPM = 20,
                Temperature = 75,
                Timestamp = DateTimeOffset.Now
            };
            byte[] msgBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(msg);
            endpoint.SendAsync("Turbine1", msgBytes);

            var rtInstances = wb.GetInstances<RealTimeWindTurbineModel>("RealTimeWindTurbine");
            var rtTurbine1 = rtInstances["Turbine1"];
            Assert.Equal(20, rtTurbine1.RPMs.Last());
        }
    }
}
