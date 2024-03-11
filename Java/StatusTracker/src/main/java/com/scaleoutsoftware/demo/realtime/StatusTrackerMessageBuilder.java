/*
 * (C) Copyright 2024 by ScaleOut Software, Inc.
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
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGMENT ARE EXPRESSLY
 * DISCLAIMED.  SOME JURISDICTIONS DO NOT ALLOW THE EXCLUSION OF IMPLIED
 * WARRANTIES, SO THE ABOVE EXCLUSIONS MAY NOT APPLY TO YOU.  IN NO
 * EVENT WILL SSI BE LIABLE TO ANY PARTY FOR ANY DIRECT, INDIRECT,
 * SPECIAL OR OTHER CONSEQUENTIAL DAMAGES FOR ANY USE OF THE SAMPLE CODE
 * INCLUDING, WITHOUT LIMITATION, ANY LOST PROFITS, BUSINESS
 * INTERRUPTION, LOSS OF PROGRAMS OR OTHER DATA ON YOUR INFORMATION
 * HANDLING SYSTEM OR OTHERWISE, EVEN IF WE ARE EXPRESSLY ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGES.
 */
package com.scaleoutsoftware.demo.realtime;

public class StatusTrackerMessageBuilder {
    private String _messageType;
    private String _id;
    private String _nodeCondition;
    private String _nodeType;
    private String _region;
    private double _latitude;
    private double _longitude;
    public StatusTrackerMessageBuilder(String id) {
        _messageType    = "";
        _id             = id;
        _nodeCondition  = "";
        _nodeType       = "";
        _region         = "";
        _latitude       = 0.0d;
        _longitude      = 0.0d;
    }

    public StatusTrackerMessageBuilder setMessageType(String type) {
        _messageType = type;
        return this;
    }

    public StatusTrackerMessageBuilder setNodeId(String id) {
        _id = id;
        return this;
    }

    public StatusTrackerMessageBuilder setNodeCondition(String condition) {
        _nodeCondition = condition;
        return this;
    }

    public StatusTrackerMessageBuilder setNodeType(String nodeType) {
        _nodeType = nodeType;
        return this;
    }

    public StatusTrackerMessageBuilder setNodeRegion(String region) {
        _region = region;
        return this;
    }

    public StatusTrackerMessageBuilder setLatitude(double latitude) {
        _latitude = latitude;
        return this;
    }

    public StatusTrackerMessageBuilder setLongitude(double longitude) {
        _longitude = longitude;
        return this;
    }

    public StatusTrackerMessage build() {
        return new StatusTrackerMessage(_messageType, _id, _region, _nodeCondition, _nodeType, _latitude, _longitude);
    }
}
