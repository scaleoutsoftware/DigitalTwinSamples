package com.scaleoutsoftware.demo.inducer;

import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;

import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;

public class Attacker extends DigitalTwinBase {
    List<String> idsToAttack = new LinkedList<>();

    public Attacker() {}

    public Attacker(String ... attackIds) {
        idsToAttack.addAll(Arrays.asList(attackIds));

    }
}
