package com.scaleoutsoftware.demo.inducer;

import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;

public class AttackerMessage {
    List<String> ids = new LinkedList<>();
    public AttackerMessage() {}
    public AttackerMessage(String ... attackIds) {
        ids.addAll(Arrays.asList(attackIds));

    }
}
