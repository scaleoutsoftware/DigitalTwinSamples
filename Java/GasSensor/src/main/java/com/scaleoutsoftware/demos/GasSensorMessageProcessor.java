/**
 * © Copyright 2024 by ScaleOut Software, Inc.
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

import com.google.gson.Gson;
import com.scaleoutsoftware.digitaltwin.core.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;

import java.nio.charset.StandardCharsets;

public class GasSensorMessageProcessor extends MessageProcessor<GasSensor, GasSensorMessage> {
    @Override
    public ProcessingResult processMessages(ProcessingContext processingContext,
                                            GasSensor gasSensor,
                                            Iterable<GasSensorMessage> messages) throws Exception {
        for (GasSensorMessage msg : messages)
        {
            gasSensor.setLastPpmReading(msg.getPpmReading());
            gasSensor.setLastPpmTime(msg.getTimestamp());

            if (msg.getPpmReading() > GasSensor.MAX_READING_ALLOWED_PPM) // handles 50+
            {
                if (!gasSensor.isLimitExceeded())
                {
                    gasSensor.setLimitExceeded(true);
                    gasSensor.setLimitStartTime(msg.getTimestamp());
                    gasSensor.incrementNumEvents();
                }
                if ((gasSensor.getLastPpmTime() - gasSensor.getLimitStartTime()) > GasSensor.MAX_READING_ALLOWED_LIMIT_TIME_SECS ||
                        gasSensor.getLastPpmReading() >= GasSensor.MAX_PPM_READING_SPIKE)
                {
                    gasSensor.setAlarmSounded(true);
                    Gson gson = new Gson();
                    GasSensorAlert alert = new GasSensorAlert("Warning: dangerous air quality.", System.currentTimeMillis());
                    String serializedMsg = gson.toJson(alert);
                    processingContext.sendToDataSource(serializedMsg.getBytes(StandardCharsets.UTF_8));
                    //processingContext.sendToDigitalTwin("NaturalGasMeterManager", "23", "");
                }
            } else if(gasSensor.isLimitExceeded()) {
                gasSensor.setLimitExceeded(false);
            }
        }
        return ProcessingResult.UpdateDigitalTwin;
    }
}
