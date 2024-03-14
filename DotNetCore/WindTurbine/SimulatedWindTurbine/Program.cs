using Scaleout.DigitalTwin.Hosting;
using Scaleout.InvocationGrid.Hosting;

namespace SimulatedWindTurbine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "--debug")
            {
                DebugHelper.StartIG(igName: "__SimulatedSimulatedWindTurbine_IG",
                                    startupParam: null,
                                    startupSignalPort: 45845,
                                    devConnectionString: "bootstrapGateways=localhost:721;ignoreKeyAppId=true");
            }

            Startup.RunWithSimulationSupport(new SimulatedWindTurbineMessageProcessor(), new SimulatedWindTurbineProcessor(), args);

        }
    }
}
