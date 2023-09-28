package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;

import java.io.Serializable;

public class RealTimeCarMessageProcessor extends MessageProcessor<RealTimeCar, TirePressureMessage> implements Serializable {
    final int TIRE_PRESSURE_FULL = 100;
    @Override
    public ProcessingResult processMessages(ProcessingContext processingContext, RealTimeCar car, Iterable<TirePressureMessage> messages) throws Exception {
        // apply the updates from the messages
        for(TirePressureMessage message : messages) {
            car.incrementTirePressure(message.getPressureChange());
        }
        if(car.getTirePressure() > TIRE_PRESSURE_FULL) {
            processingContext.sendToDataSource(new TirePressureMessage(car.getTirePressure()));
        }
        return ProcessingResult.UpdateDigitalTwin;
    }
}