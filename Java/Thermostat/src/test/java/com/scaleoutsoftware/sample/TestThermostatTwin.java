/*
 * (C) Copyright 2023 by ScaleOut Software, Inc.
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
package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;
import com.scaleoutsoftware.digitaltwin.core.SimulationStatus;
import com.scaleoutsoftware.digitaltwin.development.SimulationStep;
import com.scaleoutsoftware.digitaltwin.development.Workbench;
import com.scaleoutsoftware.digitaltwin.development.WorkbenchException;
import com.scaleoutsoftware.sample.*;
import org.junit.Test;

import java.util.HashMap;

public class TestThermostatTwin {
    @Test
    public void testThermostatTwin() throws Exception {
        try (Workbench workbench = new Workbench()) {
            workbench.addRealTimeModel("Thermostat", new RealTimeThermostatMessageProcessor(), RealTimeThermostat.class, TemperatureChangeMessage.class);
            workbench.addSimulationModel("SimHeater", new HeaterMessageProcessor(), new HeaterSimulationProcessor(), SimulatedHeater.class, TemperatureChangeMessage.class);
            workbench.addInstance("SimHeater", "19", new SimulatedHeater());
            workbench.addInstance("Thermostat", "19", new RealTimeThermostat());
            SimulationStep step = workbench.initializeSimulation(System.currentTimeMillis(), System.currentTimeMillis() + 60000, 1000);
            while (step.getStatus() == SimulationStatus.Running) {
                step = workbench.step();
            }
            workbench.generateModelSchema("Thermostat", "C:\\Users\\brandonr\\Desktop");
        }
    }
}
