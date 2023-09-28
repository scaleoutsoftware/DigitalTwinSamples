package com.scaleoutsoftware.sample;

import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;

public class RealTimeCar extends DigitalTwinBase {
    private int _tirePressure;
    public RealTimeCar() { _tirePressure=0; }
    public RealTimeCar(int startingTirePressure) {
        _tirePressure = startingTirePressure;
    }

    public void incrementTirePressure(int increment) {
        _tirePressure += increment;
    }

    public int getTirePressure() {
        return _tirePressure;
    }
}