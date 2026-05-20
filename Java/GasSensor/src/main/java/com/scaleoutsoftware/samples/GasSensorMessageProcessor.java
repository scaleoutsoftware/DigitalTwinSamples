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

import com.google.gson.Gson;
import com.scaleoutsoftware.digitaltwin.abstractions.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.abstractions.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.abstractions.ProcessingResult;

import java.io.PrintWriter;
import java.io.StringWriter;
import java.nio.charset.StandardCharsets;
import java.util.logging.Level;
public class GasSensorMessageProcessor extends MessageProcessor<GasSensor> {

    @Override
    public ProcessingResult processMessage(ProcessingContext<GasSensor> processingContext, GasSensor gasSensor, byte[] message) {
        try {
            // Add context-aware processing logic that makes use of the soss object here.
            // EXAMPLE: Deserialize the message and update the object
            GasSensorMessage msg = GasSensorMessage.deserialize(message);
            gasSensor.setLastPpmReading(msg.getPpmReading());
            gasSensor.setLastPpmTime(msg.getTimestamp());

            if (msg.getPpmReading() > GasSensor.MAX_READING_ALLOWED_PPM) // handles 50+
            {
                if (!gasSensor.isLimitExceeded())
                {
                    gasSensor.setLimitExceeded(true);
                    gasSensor.setLimitStartTime(msg.getTimestamp());
                }
                if (((gasSensor.getLastPpmTime() - gasSensor.getLimitStartTime())/1000) > GasSensor.MAX_READING_ALLOWED_LIMIT_TIME_SECS ||
                        gasSensor.getLastPpmReading() >= GasSensor.MAX_PPM_READING_SPIKE)
                {
                    gasSensor.setAlarmSounded(true);
                    Gson gson = new Gson();
                    GasSensorAlert alert = new GasSensorAlert("Warning: dangerous air quality.", 100);
                    String serializedMsg = gson.toJson(alert);
                    processingContext.sendToDataSource(serializedMsg.getBytes(StandardCharsets.UTF_8));
                }
            } else if(gasSensor.isLimitExceeded()) {
                gasSensor.setLimitExceeded(false);
                gasSensor.setAlarmSounded(false);
            }
        } catch (Exception e) {
            // Catch all exceptions and send an alert using the UI alerter.
            StringWriter sw = new StringWriter();
            PrintWriter pw = new PrintWriter(sw);
            e.printStackTrace(pw);
            pw.flush();
            sw.flush();
            processingContext.logMessage(Level.SEVERE, "Exception thrown by id" + processingContext.getDataSourceId() + " " + sw.toString());
        }
        // Return ProcessingResult.UpdateDigitalTwin if this method modified the twin instance.
        // If no changes occurred or the changes are to be discarded, return ProcessingResult.NoUpdate.
        return ProcessingResult.UpdateDigitalTwin;
    }
}