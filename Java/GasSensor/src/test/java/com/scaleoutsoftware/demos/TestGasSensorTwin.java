/**
 * © Copyright 2020 by ScaleOut Software, Inc.
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
package com.scaleoutsoftware.demos;

import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;
import com.scaleoutsoftware.digitaltwin.development.Workbench;
import org.junit.Assert;
import org.junit.Test;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;

public class TestGasSensorTwin {
    @Test
    public void test30SecondPpmLimit() {
        String modelName = "GasSensor";
        String id = "19";
        long startTime = System.currentTimeMillis();
        long in25Seconds = startTime+25000;
        long in31Seconds = startTime+31000;
        try (Workbench workbench = new Workbench()) {
            workbench.addRealTimeModel("GasSensor", new GasSensorTwinMessageProcessor(), GasSensorTwin.class, GasSensorTwinMessage.class);
            workbench.addInstance("GasSensor", "19", new GasSensorTwin());
            List<Object> messages = new LinkedList<>();
            messages.add(new GasSensorTwinMessage(75, startTime));
            workbench.send(modelName, id, messages);
            messages.clear();
            messages.add(new GasSensorTwinMessage(75, in25Seconds));
            workbench.send(modelName, id, messages);
            HashMap<String, DigitalTwinBase> instances = workbench.getInstances(modelName);
            GasSensorTwin gasSensorTwin = (GasSensorTwin)instances.get(id);
            Assert.assertFalse(gasSensorTwin.isAlarmSounded());
            Assert.assertTrue(gasSensorTwin.isLimitExceeded());
            messages.clear();
            messages.add(new GasSensorTwinMessage(75, in31Seconds));
            workbench.send(modelName, id, messages);
            instances = workbench.getInstances(modelName);
            gasSensorTwin = (GasSensorTwin)instances.get(id);
            Assert.assertTrue(gasSensorTwin.isLimitExceeded());
            Assert.assertTrue(gasSensorTwin.isAlarmSounded());
        } catch (Exception e) {
            Assert.fail(e.getMessage());
        }
    }

    @Test
    public void test200ppmSpike() {
        String modelName = "GasSensor";
        String id = "19";
        long startTime = System.currentTimeMillis();
        try (Workbench workbench = new Workbench()) {
            workbench.addRealTimeModel("GasSensor", new GasSensorTwinMessageProcessor(), GasSensorTwin.class, GasSensorTwinMessage.class);
            workbench.addInstance("GasSensor", "19", new GasSensorTwin());
            List<Object> messages = new LinkedList<>();
            messages.add(new GasSensorTwinMessage(200, startTime));
            workbench.send(modelName, id, messages);
            HashMap<String, DigitalTwinBase> instances = workbench.getInstances(modelName);
            GasSensorTwin gasSensorTwin = (GasSensorTwin)instances.get(id);
            Assert.assertTrue(gasSensorTwin.isAlarmSounded());
        } catch (Exception e) {
            Assert.fail(e.getMessage());
        }
    }


}
