package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;

public class SimulatedPump extends DigitalTwinBase {
    private double _tirePressureChange;
    private boolean _tirePressureReached = false;
    public SimulatedPump() {}
    public SimulatedPump(double pressureChange) {
        _tirePressureChange = pressureChange;
    }

    public double getTirePressureChange() {
        return _tirePressureChange;
    }

    public void setTirePressureReached() {
        _tirePressureReached = true;
    }

    public boolean isTireFull() {
        return _tirePressureReached;
    }
}
