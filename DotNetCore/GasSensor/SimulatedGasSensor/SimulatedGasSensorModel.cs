/* 
 * © Copyright 2024 by ScaleOut Software, Inc.
 *
 * LICENSE AND DISCLAIMER
 * ----------------------
 * This material contains sample programming source code ("Sample Code").
 * ScaleOut Software, Inc. (SSI) grants you a nonexclusive license to compile, 
 * link, run, display, reproduce, and prepare derivative works of 
 * this Sample Code.  The Sample Code has not been thoroughly
 * tested under all conditions.  SSI, therefore, does not guarantee
 * or imply its reliability, serviceability, or function. SSI
 * provides no support services for the Sample Code.
 *
 * All Sample Code contained herein is provided to you "AS IS" without
 * any warranties of any kind. THE IMPLIED WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGMENT ARE EXPRESSLY
 * DISCLAIMED.  SOME JURISDICTIONS DO NOT ALLOW THE EXCLUSION OF IMPLIED
 * WARRANTIES, SO THE ABOVE EXCLUSIONS MAY NOT APPLY TO YOU.  IN NO 
 * EVENT WILL SSI BE LIABLE TO ANY PARTY FOR ANY DIRECT, INDIRECT, 
 * SPECIAL OR OTHER CONSEQUENTIAL DAMAGES FOR ANY USE OF THE SAMPLE CODE
 * INCLUDING, WITHOUT LIMITATION, ANY LOST PROFITS, BUSINESS 
 * INTERRUPTION, LOSS OF PROGRAMS OR OTHER DATA ON YOUR INFORMATION
 * HANDLING SYSTEM OR OTHERWISE, EVEN IF WE ARE EXPRESSLY ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGES.
 */
using Scaleout.Streaming.DigitalTwin.Core;
using ScaleOut.DigitalTwin.Samples.GasSensor.Messages;

namespace ScaleOut.DigitalTwin.Samples.SimulatedGasSensor
{
    /// <summary>
    /// The simulated gas sensor twin model. It simulates real IoT devices
    /// that are periodically emmiting telemetry to their real-time counterparts.
    /// </summary>
    public class SimulatedGasSensorModel : DigitalTwinBase
    {
        #region Sensor State properties
        public int CurrentPPMValue { get; set; } = 0;

        public int NumberOfSimIterations { get; set; } = 0;

        public SensorStatus SensorStatus { get; set; }
        #endregion

        #region Digital Twin Geo properties
        /// <summary>The site name (e.g. Seattle site, etc.) where the gas sensor is located.</summary>
        public string Site { get; set; }

        /// <summary>Gas sensor location's latitude.</summary>
        public decimal Latitude { get; set; }

        /// <summary>Gas sensor location's longitude.</summary>
        public decimal Longitude { get; set; }
        #endregion

        /// <summary>
        /// Illustrates how a digital twin specific initialization logic 
        /// can be added here, which will be called once at the object creation time.
        /// It randomly assigns the state and the long/lat pair for one of the major cities 
        /// in each state.
        /// </summary>
        /// <param name="id">Digital twin identifier.</param>
        /// <param name="model">Digital twin model name.</param>
        public override void Init(string id, string model)
        {
            base.Init(id, model);
            SensorStatus = SensorStatus.Active;
        }
    }
}
