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
using System;

using Scaleout.Streaming.DigitalTwin.Core;

namespace ScaleOut.DigitalTwin.Samples.GasSensorTwin
{
    /// <summary>
    /// Model holding that holds state for a real-time
    /// digital twin instance.
    /// </summary>
    public class GasSensorTwinModel : DigitalTwinBase
    {
        #region Constant values
        public const int MaxAllowedPPM = 50;
        public static TimeSpan MaxAllowedTimePeriod = TimeSpan.FromSeconds(30);
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
        public string Site { get; set; }

        /// <summary>Gas sensor location's latitude.</summary>
        public decimal Latitude { get; set; }

        /// <summary>Gas sensor location's longitude.</summary>
        public decimal Longitude { get; set; }
        #endregion
    }
}
