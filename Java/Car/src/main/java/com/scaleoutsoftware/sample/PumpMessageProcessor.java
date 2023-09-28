package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;

import java.io.Serializable;

public class PumpMessageProcessor extends MessageProcessor<SimulatedPump, TirePressureMessage> implements Serializable {
    @Override
    public ProcessingResult processMessages(ProcessingContext processingContext, SimulatedPump pump, Iterable<TirePressureMessage> messages) throws Exception {
        // apply the updates from the messages
        pump.setTirePressureReached();
        return ProcessingResult.UpdateDigitalTwin;
    }
}