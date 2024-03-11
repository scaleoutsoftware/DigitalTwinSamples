/*
 * (C) Copyright 2024 by ScaleOut Software, Inc.
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
package com.scaleoutsoftware.demo.simulation;

import com.scaleoutsoftware.demo.Constants;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessage;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessageBuilder;
import com.scaleoutsoftware.digitaltwin.core.*;

import java.nio.charset.StandardCharsets;
import java.time.Duration;
import java.util.Date;
import java.util.Random;
import java.util.logging.Level;

public class DataSourceSimulationProcessor extends SimulationProcessor<DataSource> {

    private static Random RANDOM = new Random();

    @Override
    public ProcessingResult processModel(ProcessingContext processingContext, DataSource dataSource, Date epoch) {
        SimulationController simulation = processingContext.getSimulationController();
        if(simulation != null) {
            long delayMs = 0L;
            StatusTrackerMessage message = null;
            // simulation is starting
            if (dataSource.getNextSimulationTimeMs() == 0L) {
                message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                        .setNodeType(dataSource.getNodeType())
                        .setNodeRegion(dataSource.getRegion())
                        .setLatitude(dataSource.getLatitude())
                        .setLongitude(dataSource.getLongitude())
                        .setMessageType(Constants.MESSAGE_TYPE_INIT)
                        .build();
                delayMs = Constants.SIMULATION_DELAY_NORMAL;
            } else {
                /* if we have been asynchronously attacked, send message and delay accordingly */
                if (dataSource.attacked()) {
                    processingContext.logMessage(Level.SEVERE, String.format("DataSource (%s) asynchronously attacked! (%s)", processingContext.getDataSourceId(), epoch.getTime()));
                    dataSource.setStatus(Constants.NODE_CONDITION_SEVERE);
                    dataSource.setAttacked(false);
                    message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                            .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                            .setNodeCondition(Constants.NODE_CONDITION_SEVERE)
                            .build();
                    delayMs = Constants.SIMULATION_DELAY_SEVERE;
                } else if (dataSource.getStatus().compareTo(Constants.NODE_CONDITION_SEVERE) == 0) {
                    processingContext.logMessage(Level.INFO, String.format("DataSource (%s) stops tracking attack. (%s)", processingContext.getDataSourceId(), epoch.getTime()));
                    /* reset datasource; mark offline. */
                    dataSource.setStatus(Constants.NODE_CONDITION_OFFLINE);
                    message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                            .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                            .setNodeCondition(Constants.NODE_CONDITION_OFFLINE)
                            .build();
                    delayMs = Constants.SIMULATION_DELAY_OFFLINE;
                }
                /* compute if we should update our status... */
                else if (calculateState(Constants.SIMULATION_PR_OFFLINE)) {
                    dataSource.setStatus(Constants.NODE_CONDITION_OFFLINE);
                    message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                            .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                            .setNodeCondition(Constants.NODE_CONDITION_OFFLINE)
                            .build();
                    delayMs = Constants.SIMULATION_DELAY_OFFLINE;
                } else if (calculateState(Constants.SIMULATION_PR_NORMAL)) {
                    dataSource.setStatus(Constants.NODE_CONDITION_NORMAL);
                    message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                            .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                            .setNodeCondition(Constants.NODE_CONDITION_NORMAL)
                            .build();
                    delayMs = Constants.SIMULATION_DELAY_NORMAL;
                } else if (calculateState(Constants.SIMULATION_PR_MINOR)) {
                    dataSource.setStatus(Constants.NODE_CONDITION_MINOR);
                    message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                            .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                            .setNodeCondition(Constants.NODE_CONDITION_MINOR)
                            .build();
                    delayMs = Constants.SIMULATION_DELAY_MINOR;
                } else if (calculateState(Constants.SIMULATION_PR_MODERATE)) {
                    dataSource.setStatus(Constants.NODE_CONDITION_MODERATE);
                    message = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                            .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                            .setNodeCondition(Constants.NODE_CONDITION_MODERATE)
                            .build();
                    delayMs = Constants.SIMULATION_DELAY_MODERATE;
                }

                else if (dataSource.getStatus().compareTo(Constants.NODE_CONDITION_OFFLINE) == 0) {
                    delayMs = Constants.SIMULATION_DELAY_OFFLINE;
                } else if (dataSource.getStatus().compareTo(Constants.NODE_CONDITION_NORMAL) == 0) {
                    delayMs = Constants.SIMULATION_DELAY_NORMAL;
                } else if (dataSource.getStatus().compareTo(Constants.NODE_CONDITION_MINOR) == 0) {
                    delayMs = Constants.SIMULATION_DELAY_MINOR;
                } else if (dataSource.getStatus().compareTo(Constants.NODE_CONDITION_MODERATE) == 0) {
                    delayMs = Constants.SIMULATION_DELAY_MODERATE;
                } else if (dataSource.getStatus().compareTo(Constants.NODE_CONDITION_SEVERE) == 0) {
                    delayMs = Constants.SIMULATION_DELAY_SEVERE;
                }
            }

            if(message != null) {
                simulation.emitTelemetry(Constants.REALTIME_MODEL_NAME, message.toBytes());
            }

            simulation.delay(Duration.ofMillis(delayMs));
        }
        return ProcessingResult.UpdateDigitalTwin;
    }

    private boolean calculateState(int probability) {
        int rVal = Math.abs(RANDOM.nextInt(0x7fff));
        int	max = 0x7fff * probability / 100;
        return rVal <= max;
    }
}
