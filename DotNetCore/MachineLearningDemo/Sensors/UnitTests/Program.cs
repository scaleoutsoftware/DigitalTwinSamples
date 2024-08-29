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
using Scaleout.DigitalTwin.Workbench;
using Scaleout.DigitalTwin.Workbench.MachineLearning;
using SensorsRT;
using System;

namespace UnitTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create our Workbench test environment
            using RealTimeWorkbench wb = new RealTimeWorkbench();

            // Add our trained algorithm for SensorsRT called Overheating, using the zip
            // file produced by the ScaleOut Machine Learning Training Tool. For the purpose
            // of this sample, we binplace the zip file to the binaries folder of the test app.
            wb.AddAnomalyDetectionProvider("SensorsRT", "Overheating", "Overheating.zip", null);

            // Add our SensorsRT model
            var endpoint = wb.AddRealTimeModel("SensorsRT", new SensorsRTMessageProcessor());

            // Send a message to an instance. This message should result in an anomaly
            var msg = new SensorsMessage()
            {
                RPM = 42500f,
                Temperature = 228f,
                Friction = 2.31f
            };
            endpoint.Send("InstanceA", msg);

            // There are lots of ways to validate the behavior of the ProcessMessages code.
            // Here, to keep things simple, we will just display in the Console the resulting
            // value for Status, which should be "Anomaly detected" since we sent values that
            // should get flagged as an anomaly. You could integrate with your favorite test
            // framework to create an actual test.
            var instances = wb.GetInstances<SensorsRTModel>("SensorsRT");
            if (instances != null)
            {
                var instance = instances.ContainsKey("InstanceA") ? instances["InstanceA"] : null;
                if (instance != null)
                {
                    Console.WriteLine(instance.Status);
                }
            }
        }
    }
}