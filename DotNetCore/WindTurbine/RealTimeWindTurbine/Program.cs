using Scaleout.DigitalTwin.Hosting;
using Scaleout.InvocationGrid.Hosting;

namespace RealTimeWindTurbine
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLower() == "--debug")
            {
                DebugHelper.StartIG(igName: "__RealTimeRealTimeWindTurbine_IG",
                                    startupParam: null,
                                    startupSignalPort: 45845,
                                    devConnectionString: "bootstrapGateways=localhost:721;ignoreKeyAppId=true");
            }

            Startup.Run(new RealTimeWindTurbineMessageProcessor(), args);

        }
    }
}
