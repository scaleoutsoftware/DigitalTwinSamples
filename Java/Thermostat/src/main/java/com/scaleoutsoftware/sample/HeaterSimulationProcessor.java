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

import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;
import com.scaleoutsoftware.digitaltwin.core.SimulationController;
import com.scaleoutsoftware.digitaltwin.core.SimulationProcessor;
import com.scaleoutsoftware.sample.car.SimulatedPump;
import com.scaleoutsoftware.sample.car.TirePressureMessage;

import java.time.Duration;
import java.util.Date;
import java.util.Random;

public class HeaterSimulationProcessor extends SimulationProcessor<SimulatedHeater> {
    public ProcessingResult processModel(ProcessingContext processingContext, SimulatedHeater heater, Date date) {
        SimulationController controller = processingContext.getSimulationController();
        if(heater.isShutdown()) {
            controller.deleteThisInstance();
        } else if(heater.increaseTemperature()) {
            Random random = new Random();
            int change = random.nextInt(3);
            controller.emitTelemetry("Thermostat", new TemperatureChangeMessage(change));
            heater.setIncreaseTemperature(false);
            controller.delay(Duration.ofMillis(SimulatedHeater.HEATER_ACTIVE_DELAY_MS));
        } else if (!heater.increaseTemperature()) {
            heater.setIncreaseTemperature(true);
            controller.delay(Duration.ofMillis(SimulatedHeater.HEATER_INACTIVE_DELAY_MS));
        }
        return ProcessingResult.UpdateDigitalTwin;
    }
}
