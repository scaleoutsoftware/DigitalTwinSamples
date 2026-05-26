/**
 * © Copyright 2026 by ScaleOut Software, Inc.
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
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT ARE EXPRESSLY
 * DISCLAIMED.  SOME JURISDICTIONS DO NOT ALLOW THE EXCLUSION OF IMPLIED
 * WARRANTIES, SO THE ABOVE EXCLUSIONS MAY NOT APPLY TO YOU.  IN NO
 * EVENT WILL SSI BE LIABLE TO ANY PARTY FOR ANY DIRECT, INDIRECT,
 * SPECIAL OR OTHER CONSEQUENTIAL DAMAGES FOR ANY USE OF THE SAMPLE CODE
 * INCLUDING, WITHOUT LIMITATION, ANY LOST PROFITS, BUSINESS
 * INTERRUPTION, LOSS OF PROGRAMS OR OTHER DATA ON YOUR INFORMATION
 * HANDLING SYSTEM OR OTHERWISE, EVEN IF WE ARE EXPRESSLY ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGES.
 */
package com.scaleoutsoftware.samples;

import com.scaleoutsoftware.digitaltwin.abstractions.DigitalTwinBase;
import com.scaleoutsoftware.digitaltwin.development.Workbench;

import org.junit.Assert;
import org.junit.Test;

import java.util.HashMap;

/**
 * Unit tests for module development.
 */
public class TestDigitalTwin
{

    @Test
    public void testMessageBasedInitialization() {
        try (Workbench workbench = new Workbench()) {
            String modelName          = "GasSensor";
            String testId             = "23";

            // Add the DigitalTwinImpl real-time model to the workbench.
            workbench.addRealTimeModel(modelName, new GasSensorMessageProcessor(), GasSensor.class);

            // Create a message list with an initialization message
            GasSensorMessage message = new GasSensorMessage(5, System.currentTimeMillis());
            byte[] serializedMsg = GasSensorMessage.serialize(message);

            // Send the message list to the workbench. The workbench is responsible for instantiating the model's instance
            // and delivering the message.
            workbench.send(modelName, testId, serializedMsg);

            // Introspect on the state of the DigitalTwinImpl instance.
            HashMap<String, DigitalTwinBase<?>> instances = workbench.getInstances(modelName);
            GasSensor instance = (GasSensor)instances.get(testId);
            Assert.assertNotNull(instance);
        } catch (Exception e) {
            Assert.fail(e.getMessage());
        }
    }
}
