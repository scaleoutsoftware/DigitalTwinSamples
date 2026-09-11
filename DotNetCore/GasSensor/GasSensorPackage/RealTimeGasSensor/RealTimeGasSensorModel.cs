using Scaleout.Modules.DigitalTwin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasSensorPackage.RealTimeGasSensor
{
    /// <summary>
    /// Model holding that holds state for a real-time
    /// digital twin instance.
    /// </summary>
    public class RealTimeGasSensorModel : DigitalTwinBase<RealTimeGasSensorModel>
    {
        #region Constant values
        public const int MaxAllowedPPM = 50;
        public static TimeSpan MaxAllowedTimePeriod = TimeSpan.FromSeconds(4);
        public const int SpikeAlertPPM = 200;
        #endregion

        #region Digital Twin State properties
        public int LastPPMReading { get; set; }
        public DateTime LastPPMTime { get; set; }

        public bool LimitExceeded { get; set; }
        public int AlarmSounded { get; set; } // alerted state is 1, normal state is 0
        public DateTime LimitStartTime { get; set; }
        #endregion

        #region Digital Twin Geo properties
        /// <summary>The site name (e.g. Seattle site, etc.) where the gas sensor is located.</summary>
        public string? Site { get; set; }

        /// <summary>Gas sensor location's latitude.</summary>
        public decimal Latitude { get; set; }

        /// <summary>Gas sensor location's longitude.</summary>
        public decimal Longitude { get; set; }
        #endregion


        public override async Task InitAsync(string id, string model, InitContext<RealTimeGasSensorModel> initContext)
        {
            await base.InitAsync(id, model);

            // Initialization logic for a digital twin instance goes here.
            // This method is called when a new digital twin instance is created.
        }

    }
}
