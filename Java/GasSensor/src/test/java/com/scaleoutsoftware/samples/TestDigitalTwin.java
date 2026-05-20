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
