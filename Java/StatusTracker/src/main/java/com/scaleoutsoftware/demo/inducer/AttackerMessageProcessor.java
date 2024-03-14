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
package com.scaleoutsoftware.demo.inducer;

import com.scaleoutsoftware.demo.Constants;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessage;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessageBuilder;
import com.scaleoutsoftware.demo.simulation.DataSourceMessage;
import com.scaleoutsoftware.digitaltwin.core.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;

public class AttackerMessageProcessor extends MessageProcessor<Attacker,AttackerMessage> {
    @Override
    public ProcessingResult processMessages(ProcessingContext processingContext, Attacker attacker, Iterable<AttackerMessage> messages) throws Exception {
        for(AttackerMessage msg : messages) {
            for(String id : msg.ids) {
                StatusTrackerMessage statusTrackerMessage = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                        .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                        .setNodeCondition(Constants.NODE_CONDITION_SEVERE)
                        .build();
                processingContext.sendToDigitalTwin("StatusTracker", id, statusTrackerMessage);
                DataSourceMessage dataSourceMessage = new DataSourceMessage(true);
                processingContext.sendToDigitalTwin("DataSource", id, dataSourceMessage);
            }
        }
        return ProcessingResult.UpdateDigitalTwin;
    }
}
