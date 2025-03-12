/*
 * (C) Copyright 2025 by ScaleOut Software, Inc.
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

using Messages;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealTimeTwin
{
    /// <summary>
    /// Handles messages sent to a real-time digital twin instance.
    /// </summary>
    public class MyRealTimeTwinMessageProcessor : MessageProcessor<MyRealTimeTwinModel, MyRealTimeTwinMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, MyRealTimeTwinModel digitalTwin, IEnumerable<MyRealTimeTwinMessage> newMessages)
        {
            try
            {
                // Process incoming messages
                foreach (var message in newMessages)
                {
                    switch (message)
                    {
                        case MyRealTimeTwinMessage exampleMessage:
                            // Update the digital twin's current value
                            digitalTwin.currentValue = exampleMessage.StringPayload;
                            // Log an informational message
                            context.LogMessage(
                                LogSeverity.Informational,
                                $"The real-time digital twin '{digitalTwin.Id}' says '{digitalTwin.currentValue}'"
                            );
                            break;
                        default:
                            throw new NotImplementedException($"Message processor does not support message type {message.GetType()}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Catch all exceptions and log them using the embedded logger.
                // If the ScaleOut Digital Twin Streaming Service is used to host real-time digital twins, 
                // you can see both trace messages and logged exceptions on the Manage Digital Twin Model page.
                context.LogMessage(LogSeverity.Error,
                       string.Format("Exception occurred while processing new messages for the real-time digital twin object '{0}'. Details: {1}",
                digitalTwin.Id, ex.Message));
            }

            // Return ProcessingResult.DoUpdate if this method modified the state of this digital twin instance 
            // to persist the changes back to ScaleOut StreamServer;
            // otherwise, if no changes occurred or the changes are to be discarded, return ProcessingResult.NoUpdate.
            return ProcessingResult.DoUpdate;
        }
    }
}
