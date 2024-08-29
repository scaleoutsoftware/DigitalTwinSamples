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

using Messages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Scaleout.Streaming.DigitalTwin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorsRT
{
    /// <summary>
    /// Handles messages sent to a real-time digital twin instance.
    /// </summary>
    public class SensorsRTMessageProcessor : MessageProcessor<SensorsRTModel, DigitalTwinMessage>
    {
        public override ProcessingResult ProcessMessages(ProcessingContext context, SensorsRTModel digitalTwin, IEnumerable<DigitalTwinMessage> newMessages)
        {
            try
            {
                Dictionary<string, Single> properties = new Dictionary<string, float>();
                
                // Look up the Anomaly Detection Provider by name from the context
                var algorithm = context.AnomalyDetectionProviders?["Overheating"];

                // Process incoming messages
                foreach (var message in newMessages)
                {
                    switch(message)
                    {
                        case SensorsMessage newReading:
                            // Assign new values
                            digitalTwin.Temperature = newReading.Temperature;
                            digitalTwin.RPM = newReading.RPM;
                            digitalTwin.Friction = newReading.Friction;

                            // Check for anomalies
                            if (algorithm != null)
                            {
                                // Build the dictionary of properties with the values we want to get a prediction for,
                                // in this case, values obtained from the new message.
                                properties[nameof(SensorsRTModel.Temperature)] = newReading.Temperature;
                                properties[nameof(SensorsRTModel.RPM)] = newReading.RPM;
                                properties[nameof(SensorsRTModel.Friction)] = newReading.Friction;

                                // Run the anomaly detection on the new provided values
                                if (algorithm.DetectAnomaly(properties) == true)
                                {
                                    // In this sample, log a message and update the Status property
                                    context.LogMessage(LogSeverity.Error, $"An anomaly has been detected : {DisplayProperties(properties)}");
                                    digitalTwin.Status = "Anomaly Detected";
                                }
                                else
                                {
                                    context.LogMessage(LogSeverity.Informational, $"No anomaly: {DisplayProperties(properties)}");
                                    digitalTwin.Status = "Normal";
                                }
                            }
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

        private string DisplayProperties(Dictionary<string, float> properties)
        {
            if (properties != null)
            {
                return String.Join(", ", properties.Select(p => $"{p.Key}:{p.Value}"));
            }

            return String.Empty;
        }
    }
}
