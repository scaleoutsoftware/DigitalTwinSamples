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
using System.Collections.Generic;
using System.Text;

namespace Messages
{
    /// <summary>
    /// A sample message that can be sent to MessageProcessor implementations. 
    /// </summary>
    public class SensorsMessage : DigitalTwinMessage
    {
        /// <summary>
        /// Tracking the Friction measurement that a sensor would send
        /// </summary>
        public Single Friction { get; set; }

        /// <summary>
        /// Tracking the Temperature measurement that a sensor would send
        /// </summary>
        public Single Temperature { get; set; }

        /// <summary>
        /// Tracking the Rotations Per Minute measurement that a sensor would send
        /// </summary>
        public Single RPM { get; set; }

        /// <summary>
        /// Overridden property used by JsonSubtypes during deserialization.
        /// </summary>
        /// <see href="https://static.scaleoutsoftware.com/docs/digital_twin_user_guide/software_toolkit/dt_builder/dotnetcore_api/dotnetcore_multiple_msg_types.html">Using Multiple Message Types</see>
        public override string MessageType => nameof(SensorsMessage);
    }
}
