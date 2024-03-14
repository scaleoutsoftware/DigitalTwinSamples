using Scaleout.DigitalTwin.Workbench;
using SimulatedWindTurbine;
using RealTimeWindTurbine;

namespace UnitTests
{
    public class SimulationTests
    {
        [Fact]
        public void RunSimulation()
        {
            SimulationWorkbench wb = new SimulationWorkbench();
            wb.AddSimulationModel("SimulatedWindTurbine", new SimulatedWindTurbineProcessor());
            wb.AddRealTimeModel("RealTimeWindTurbine", new RealTimeWindTurbineMessageProcessor());

            var simInstance = new SimulatedWindTurbineModel
            {
                CurrentRPMs = 30,
                CurrentTemp = 44
            };
            wb.AddInstance("Turbine1", "SimulatedWindTurbine", simInstance);

            DateTime startTime = new DateTime(2023, 1, 1);
            wb.InitializeSimulation(startTime,
                      endTime: new DateTime(2023, 1, 2),
                      simulationIterationInterval: TimeSpan.FromHours(1));

            StepResult stepResult;
            do
            {
                stepResult = wb.Step();
            } while (stepResult.SimulationStatus == SimulationStatus.Running);

            Assert.Equal(SimulationStatus.EndTimeReached, stepResult.SimulationStatus);

            var simInstances = wb.GetInstances<SimulatedWindTurbineModel>("SimulatedWindTurbine");
            var simTurbine1 = simInstances["Turbine1"];
            Assert.True(simTurbine1.CurrentRPMs > 30);

            var rtInstances = wb.GetInstances<RealTimeWindTurbineModel>("RealTimeWindTurbine");
            Assert.True(rtInstances.ContainsKey("Turbine1"));
        }
    }
}