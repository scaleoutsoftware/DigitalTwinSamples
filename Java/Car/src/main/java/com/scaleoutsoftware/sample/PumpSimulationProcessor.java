package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;
import com.scaleoutsoftware.digitaltwin.core.SimulationController;
import com.scaleoutsoftware.digitaltwin.core.SimulationProcessor;

import java.io.Serializable;
import java.util.Date;

public class PumpSimulationProcessor extends SimulationProcessor<SimulatedPump> implements Serializable {
    public ProcessingResult processModel(ProcessingContext processingContext, SimulatedPump simPump, Date date) {
        SimulationController controller = processingContext.getSimulationController();
        if(simPump.isTireFull()) {
            controller.deleteThisInstance();
        } else {
            int change = (int) (100 * simPump.getTirePressureChange());
            controller.emitTelemetry("RealTimeCar", new TirePressureMessage(change));
        }
        return ProcessingResult.UpdateDigitalTwin;
    }
}