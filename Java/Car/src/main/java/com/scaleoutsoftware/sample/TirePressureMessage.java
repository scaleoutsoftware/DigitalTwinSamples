package com.scaleoutsoftware.sample;

public class TirePressureMessage {
    final int _pressureChange;
    public TirePressureMessage(int pressureChange) {
        _pressureChange = pressureChange;
    }

    public int getPressureChange() {
        return _pressureChange;
    }
}
