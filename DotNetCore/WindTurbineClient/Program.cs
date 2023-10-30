using Scaleout.Client;
using Scaleout.DigitalTwin.Client;
using System.Text.Json;
using Messages;

namespace WindTurbineClient
{
    internal class Program
    {
        // See https://static.scaleoutsoftware.com/docs/dotnet_client/articles/configuration/connecting.html
        // for information about ScaleOut connection strings.
        private static readonly string CONN_STR = "bootstrapGateways=localhost:721";

        static void Main(string[] args)
        {
            // Connect to the ScaleOut service and create an endpoint to the 
            // WindTurbine digital twin.
            var conn = GridConnection.Connect(CONN_STR);
            var dtEndpoint = new DigitalTwinModelEndpoint(conn, "RealTimeWindTurbine");

            // Prepare a message and serialize it to JSON.
            var msg = new WindTurbineMessage
            {
                Timestamp = DateTime.UtcNow,
                RPM = 15,
                Temperature = 60
            };
            string msgJson = JsonSerializer.Serialize(msg);

            // Send the serialized message to a digital twin instance.
            dtEndpoint.Send("Wind Turbine 001", msgJson);
        }
    }
}