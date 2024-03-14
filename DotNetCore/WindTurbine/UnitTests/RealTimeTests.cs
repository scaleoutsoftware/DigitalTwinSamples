using Scaleout.DigitalTwin.Workbench;
using SimulatedWindTurbine;
using RealTimeWindTurbine;
using Messages;

namespace UnitTests
{
    public class RealTimeTests
    {
        [Fact]
        public void SendMessage()
        {
            RealTimeWorkbench wb = new RealTimeWorkbench();
            var endpoint = wb.AddRealTimeModel("RealTimeWindTurbine", new RealTimeWindTurbineMessageProcessor());

            var msg = new WindTurbineMessage
            {
                RPM = 20,
                Temperature = 75,
                Timestamp = DateTimeOffset.Now
            };
            endpoint.Send("Turbine1", msg);

            var rtInstances = wb.GetInstances<RealTimeWindTurbineModel>("RealTimeWindTurbine");
            var rtTurbine1 = rtInstances["Turbine1"];
            Assert.Equal(20, rtTurbine1.RPMs.Last());
        }
    }
}
