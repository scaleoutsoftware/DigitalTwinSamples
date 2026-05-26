/*
 * (C) Copyright 2026 by ScaleOut Software, Inc.
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
package com.scaleoutsoftware.samples;

import java.io.IOException;
import com.scaleoutsoftware.modules.hosting.ModulePackage;
import com.scaleoutsoftware.modules.hosting.DigitalTwinModelOptions;
import com.scaleoutsoftware.modules.hosting.DigitalTwinModelOptionsBuilder;
import com.scaleoutsoftware.modules.hosting.ModuleRegistrationException;

/**
 * A real-time digital twin's entry point. Run "mvn package" to build the deployable ZIP package.
 */
public class Main {
    public static void main(String[] args) {
        // instantiate the module package
        ModulePackage modulePackage = new ModulePackage();
        // define the DigitalTwinModelOptions
        DigitalTwinModelOptions<DataSource> digitalTwinModelOptions = new DigitalTwinModelOptionsBuilder<DataSource>(DataSource.class).build();
        // add the Digital Twin model to the package
        modulePackage.addSimulationDigitalTwinModel("DataSource", new DataSourceMessageProcessor(), new DataSourceSimulationProcessor(), digitalTwinModelOptions);
        try {
            // wait for events
            modulePackage.waitForEvents();
        } catch (ModuleRegistrationException e) {
            throw new RuntimeException(e);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}