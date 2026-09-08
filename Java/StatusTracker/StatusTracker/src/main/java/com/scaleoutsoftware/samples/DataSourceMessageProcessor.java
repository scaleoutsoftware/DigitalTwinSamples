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

import com.google.gson.Gson;
import com.scaleoutsoftware.digitaltwin.abstractions.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.abstractions.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.abstractions.ProcessingResult;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.nio.charset.StandardCharsets;

public class DataSourceMessageProcessor extends MessageProcessor<DataSource> {
    Gson gson = new Gson();

    @Override
    public ProcessingResult processMessage(ProcessingContext<DataSource> processingContext, DataSource dataSource, byte[] bytes) throws Exception {
        Logger logger = LogManager.getLogger(DataSourceMessageProcessor.class);

        DataSourceMessage msg = gson.fromJson(new String(bytes, StandardCharsets.UTF_8), DataSourceMessage.class);
        if(msg.attack()) {
            dataSource.setAttacked(true);
        }

        return ProcessingResult.UpdateDigitalTwin;
    }
}
