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

using Scaleout.Streaming.DigitalTwin.Core;
using ScaleOut.DigitalTwin.Samples.GasSensor.Messages;

namespace ScaleOut.DigitalTwin.Samples.SimulatedGasSensor
{
    /// <summary>
    /// Handles messages sent from a real-time digital twin. 
    /// </summary>
    /// <remarks>
    /// Messages sent here typically originate from ProcessingContext.SendToDataSource() calls made
    /// in a real-time digital twin's MessageProcessor implementation. The model
    /// in this simulation project is simulating a data source (a real-world device),
    /// so when a real-time model sends a message back to its data source during a simulation run, 
    /// the message arrives here.
    /// </remarks>
    public class SimulatedGasSensorMessageProcessor : MessageProcessor<SimulatedGasSensorModel, DigitalTwinMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, SimulatedGasSensorModel digitalTwin, IEnumerable<DigitalTwinMessage> newMessages)
        {
            //  If your simulation model receives messages, process them for the supplied digital twin.
            
            foreach (DigitalTwinMessage message in newMessages)
            {
                switch (message)
                {
                    case DeviceCommand controlMessage:
                        if (controlMessage.Code == 100) // set to Alarmed state and make it Inactive on the next simulation step
                            digitalTwin.SensorStatus = SensorStatus.Alarmed;
                        break;
                    default:
                        throw new NotImplementedException($"Message processor does not support message type {message.GetType()}");
                }
            }
            
            return ProcessingResult.DoUpdate;
        }
    }
}
