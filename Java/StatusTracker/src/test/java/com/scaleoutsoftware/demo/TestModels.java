package com.scaleoutsoftware.demo;

import com.google.gson.Gson;
import com.scaleout.client.GridConnectException;
import com.scaleout.client.GridConnection;
import com.scaleout.client.ServiceEvents;
import com.scaleout.client.caching.*;
import com.scaleoutsoftware.demo.inducer.Attacker;
import com.scaleoutsoftware.demo.inducer.AttackerMessage;
import com.scaleoutsoftware.demo.inducer.AttackerMessageProcessor;
import com.scaleoutsoftware.demo.realtime.StatusTracker;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessage;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessageBuilder;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessageProcessor;
import com.scaleoutsoftware.demo.simulation.DataSource;
import com.scaleoutsoftware.demo.simulation.DataSourceMessage;
import com.scaleoutsoftware.demo.simulation.DataSourceMessageProcessor;
import com.scaleoutsoftware.demo.simulation.DataSourceSimulationProcessor;
import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;
import com.scaleoutsoftware.digitaltwin.core.SimulationStatus;
import com.scaleoutsoftware.digitaltwin.development.SimulationStep;
import com.scaleoutsoftware.digitaltwin.development.Workbench;
import org.junit.Assert;
import org.junit.Test;

import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;

public class TestModels {
    @Test
    public void testMessageBasedInitialization() throws GridConnectException {
        try (Workbench workbench = new Workbench()) {
            String modelName          = "StatusTracker";
            String testId             = "23";
            String expectedNodeType   = "controller";
            String expectedNodeRegion = "NW";
            double expectedLatitude   = -122.10205;
            double expectedLongitude  = 47.758786;

            // Add the StatusTracker real-time model to the workbench.
            workbench.addRealTimeModel(modelName, new StatusTrackerMessageProcessor(), StatusTracker.class, StatusTrackerMessage.class);

            // Create a message list with an initialization message
            List<Object> messages = new LinkedList<>();

            StatusTrackerMessage message = new StatusTrackerMessageBuilder(testId)
                    .setNodeType("controller")
                    .setNodeRegion("NW")
                    .setLatitude(-122.10205)
                    .setLongitude(47.758786)
                    .setMessageType(Constants.MESSAGE_TYPE_INIT)
                    .build();
            messages.add(message);

            // Send the message list to the workbench. The workbench is responsible for instantiating the model's instance
            // and delivering the message.
            workbench.send(modelName, testId, messages);

            // Introspect on the state of the StatusTracker instance.
            HashMap<String, DigitalTwinBase> instances = workbench.getInstances(modelName);
            StatusTracker instance = (StatusTracker)instances.get(testId);
            Assert.assertEquals(expectedLatitude, instance.Latitude, 0.0);
            Assert.assertEquals(expectedLongitude, instance.Longitude, 0.0);
            Assert.assertEquals(expectedNodeRegion, instance.region);
            Assert.assertEquals(expectedNodeType, instance.node_type);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    @Test
    public void testSimulationBasedInitialization() {
        try (Workbench workbench = new Workbench()) {
            String simModelName       = "DataSource";
            String realTimeModelName  = "StatusTracker";
            String testId             = "23";
            String expectedNodeType   = "controller";
            String expectedNodeStatus = "blue";
            String expectedNodeRegion = "NW";
            double expectedLatitude   = -122.10205;
            double expectedLongitude  = 47.758786;

            // Add the StatusTracker real-time model to the workbench.
            workbench.addRealTimeModel(realTimeModelName, new StatusTrackerMessageProcessor(), StatusTracker.class, StatusTrackerMessage.class);
            // Add the DataSource simulation model to the workbench.
            workbench.addSimulationModel(simModelName, new DataSourceMessageProcessor(), new DataSourceSimulationProcessor(), DataSource.class, DataSourceMessage.class);

            // Add a DataSource simulation instance to the workbench.
            workbench.addInstance(simModelName, testId, new DataSource(simModelName, testId, expectedNodeType, expectedNodeStatus, expectedNodeRegion, expectedLongitude, expectedLatitude, false));

            // Initialize a simulation.
            long startTime = System.currentTimeMillis();
            long stopTime = System.currentTimeMillis()+20000L;
            long interval = 1000L;

            SimulationStep step = workbench.initializeSimulation(startTime, stopTime, interval);

            while(step.getStatus() == SimulationStatus.Running) {
                step = workbench.step();
            }
            Assert.assertEquals(SimulationStatus.EndTimeReached, step.getStatus());

            // The simulated model, DataSource, will have sent a message to the real-time StatusTracker model.
            // Verify that the status tracker instance is initialized and is tracking the properties assigned
            // by the DataSource instance.
            HashMap<String, DigitalTwinBase> instances = workbench.getInstances(realTimeModelName);
            StatusTracker instance = (StatusTracker)instances.get(testId);
            Assert.assertEquals(expectedLatitude, instance.Latitude, 0.0);
            Assert.assertEquals(expectedLongitude, instance.Longitude, 0.0);
            Assert.assertEquals(expectedNodeRegion, instance.region);
            Assert.assertEquals(expectedNodeType, instance.node_type);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    @Test
    public void testSimulationBasedAttack() {
        try (Workbench workbench = new Workbench()) {
            String simModelName       = "DataSource";
            String realTimeModelName  = "StatusTracker";
            String testId             = "23";
            String expectedNodeType   = "controller";
            String expectedNodeStatus = "blue";
            String expectedNodeRegion = "NW";
            double expectedLatitude   = -122.10205;
            double expectedLongitude  = 47.758786;
            int expectedAlertLevel    = 20;

            // Add the StatusTracker real-time model to the workbench.
            workbench.addRealTimeModel(realTimeModelName, new StatusTrackerMessageProcessor(), StatusTracker.class, StatusTrackerMessage.class);
            // Add the DataSource simulation model to the workbench.
            workbench.addSimulationModel(simModelName, new DataSourceMessageProcessor(), new DataSourceSimulationProcessor(), DataSource.class, DataSourceMessage.class);

            // Add a DataSource simulation instance to the workbench.
            workbench.addInstance(simModelName, testId, new DataSource(simModelName, testId, expectedNodeType, expectedNodeStatus, expectedNodeRegion, expectedLongitude, expectedLatitude, true));

            // Initialize a simulation.
            long startTime = System.currentTimeMillis();
            long stopTime = System.currentTimeMillis()+20000L;
            long interval = 1000L;

            SimulationStep step = workbench.initializeSimulation(startTime, stopTime, interval);

            while(step.getStatus() == SimulationStatus.Running) {
                step = workbench.step();
            }

            Assert.assertEquals(SimulationStatus.EndTimeReached, step.getStatus());

            // The simulated model, DataSource, will have sent a message to the real-time StatusTracker model.
            // Verify that the status tracker instance is initialized and is tracking the properties assigned
            // by the DataSource instance.
            HashMap<String, DigitalTwinBase> instances = workbench.getInstances(realTimeModelName);
            StatusTracker instance = (StatusTracker)instances.get(testId);
            Assert.assertEquals(expectedLatitude, instance.Latitude, 0.0);
            Assert.assertEquals(expectedLongitude, instance.Longitude, 0.0);
            Assert.assertEquals(expectedNodeRegion, instance.region);
            Assert.assertEquals(expectedNodeType, instance.node_type);
            Assert.assertEquals(expectedAlertLevel, instance.alert_level);
            HashMap<String, DigitalTwinBase> simInstances = workbench.getInstances(simModelName);
            DataSource simInstance = (DataSource)simInstances.get(testId);
            Assert.assertEquals(expectedLatitude, simInstance.getLatitude(), 0.0);
            Assert.assertEquals(expectedLongitude, simInstance.getLongitude(), 0.0);
            Assert.assertEquals(expectedNodeRegion, simInstance.getRegion());
            Assert.assertEquals(expectedNodeType, simInstance.getNodeType());
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    @Test
    public void testClient() throws GridConnectException, CacheException {
        GridConnection connection = GridConnection.connect("bootstrapGateways=localhost:721");
        Cache<String,String> cache = new CacheBuilder<String,String>(connection, "cache", String.class)
                .customSerialization(new CacheSerializer<String>() {
                    @Override
                    public byte[] serialize(String s) throws SerializationException {
                        return s.getBytes(StandardCharsets.UTF_8);
                    }
                }, new CacheDeserializer<String>() {
                    @Override
                    public String deserialize(byte[] bytes) throws DeserializationException {
                        return new String(bytes, StandardCharsets.UTF_8);
                    }
                }).build();
        CacheResponse<String,String> response = cache.read("foo");
        System.out.println("Response: " + response.getStatus());
        System.out.println("value: " + response.getValue());
    }
}

