using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasSensorPackage.SimulatedGasSensor
{
    /// <summary>
    /// Define any state for the simulation model here. This class defines the objects
    /// stored in the ScaleOut StateServer (SOSS) service that hold state for the 
    /// simulation module.
    /// </summary>
    public class SimulatedGasSensorModel : DigitalTwinBase<SimulatedGasSensorModel>
    {
        #region Sensor State properties
        public int CurrentPPMValue { get; set; } = 0;

        public int NumberOfSimIterations { get; set; } = 0;

        public SensorStatus SensorStatus { get; set; }
        #endregion

        #region Digital Twin Geo properties
        /// <summary>The site name (e.g. Seattle site, etc.) where the gas sensor is located.</summary>
        public string? Site { get; set; }

        /// <summary>Gas sensor location's latitude.</summary>
        public decimal Latitude { get; set; }

        /// <summary>Gas sensor location's longitude.</summary>
        public decimal Longitude { get; set; }
        #endregion



        public override async Task InitAsync(string id, string model, InitContext<SimulatedGasSensorModel> initContext)
        {
            await base.InitAsync(id, model);

            // Initialization logic for a simulation instance goes here.
            // This method is called when a new simulation instance is created.

            SensorStatus = SensorStatus.Active;
        }

    }
}
