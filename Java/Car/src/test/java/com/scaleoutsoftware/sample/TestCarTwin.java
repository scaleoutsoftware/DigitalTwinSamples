package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;
import com.scaleoutsoftware.digitaltwin.core.SimulationStatus;
import com.scaleoutsoftware.digitaltwin.development.SimulationStep;
import com.scaleoutsoftware.digitaltwin.development.Workbench;
import com.scaleoutsoftware.digitaltwin.development.WorkbenchException;
import com.scaleoutsoftware.sample.*;
import org.junit.Test;

import java.util.HashMap;

public class TestCarTwin {
    @Test
    public void testCarTwin() throws WorkbenchException {
        Workbench workbench = new Workbench();
        workbench.addRealTimeModel("RealTimeCar", new RealTimeCarMessageProcessor(), RealTimeCar.class, TirePressureMessage.class);
        workbench.addSimulationModel("SimPump", new PumpMessageProcessor(), new PumpSimulationProcessor(), SimulatedPump.class, TirePressureMessage.class);
        workbench.addInstance("SimPump", "23", new SimulatedPump(0.29d));
        SimulationStep step = workbench.initializeSimulation(System.currentTimeMillis(), System.currentTimeMillis()+60000, 1000);
        while(step.getStatus() == SimulationStatus.Running) {
            step = workbench.step();
            HashMap<String, DigitalTwinBase> realTimeCars = workbench.getInstances("RealTimeCar");
            RealTimeCar rtCar = (RealTimeCar) realTimeCars.get("23");
            System.out.println("rtCar: " + rtCar.getTirePressure());
        }
    }
}
