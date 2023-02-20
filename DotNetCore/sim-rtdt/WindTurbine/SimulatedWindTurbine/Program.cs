using Scaleout.DigitalTwin.Hosting;
using Scaleout.InvocationGrid.Hosting;

namespace SimulatedWindTurbine
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "--debug")
            {
                DebugHelper.StartIG(igName: "__SimulatedWindTurbine_IG",
                                    startupParam: null,
                                    startupSignalPort: 45845,
                                    devConnectionString: "bootstrapGateways=localhost:721;ignoreKeyAppId=true");
            }

            Startup.RunWithSimulationSupport(new WindTurbineSimulationMessageProcessor(), new WindTurbineSimulationProcessor(), args);

        }
    }
}
