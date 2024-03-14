/**
 * © Copyright 2024 by ScaleOut Software, Inc.
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
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT ARE EXPRESSLY
 * DISCLAIMED.  SOME JURISDICTIONS DO NOT ALLOW THE EXCLUSION OF IMPLIED
 * WARRANTIES, SO THE ABOVE EXCLUSIONS MAY NOT APPLY TO YOU.  IN NO
 * EVENT WILL SSI BE LIABLE TO ANY PARTY FOR ANY DIRECT, INDIRECT,
 * SPECIAL OR OTHER CONSEQUENTIAL DAMAGES FOR ANY USE OF THE SAMPLE CODE
 * INCLUDING, WITHOUT LIMITATION, ANY LOST PROFITS, BUSINESS
 * INTERRUPTION, LOSS OF PROGRAMS OR OTHER DATA ON YOUR INFORMATION
 * HANDLING SYSTEM OR OTHERWISE, EVEN IF WE ARE EXPRESSLY ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGES.
 */
package com.scaleoutsoftware.demos;

import com.google.gson.annotations.SerializedName;
import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;

public class GasSensorTwin extends DigitalTwinBase {
    // static constants
    public static final int MAX_READING_ALLOWED_PPM = 50;
    public static final int MAX_READING_ALLOWED_LIMIT_TIME_SECS = 30;
    public static final int	MAX_PPM_READING_SPIKE = 200;

    // state variables
    @SerializedName("LastPPMReading")
    private int		_lastPpmReading;
    @SerializedName("LastPPMTime")
    private long	_lastPpmTime;
    @SerializedName("LimitExceeded")
    private boolean	_limitExceeded = false;
    @SerializedName("AlarmSounded")
    private int	_alarmSounded  = 0; // alerted state is 1, normal state is 0
    @SerializedName("LimitStartTime")
    private long	_limitStartTime;
    @SerializedName("Site")
    private String _site;
    @SerializedName("Longitude")
    private double _longitude;
    @SerializedName("Latitude")
    private double _latitude;

    public int getLastPpmReading() {
        return _lastPpmReading;
    }

    public void setLastPpmReading(int lastPpmReading) {
        _lastPpmReading = lastPpmReading;
    }

    public long getLastPpmTime() {
        return _lastPpmTime;
    }

    public void setLastPpmTime(long lastPpmTime) {
        _lastPpmTime = lastPpmTime;
    }

    public boolean isLimitExceeded() {
        return _limitExceeded;
    }

    public void setLimitExceeded(boolean limitExceeded) {
        _limitExceeded = limitExceeded;
    }

    public boolean isAlarmSounded() {
        return _alarmSounded == 1;
    }

    public void setAlarmSounded(boolean alarmSounded) {
        _alarmSounded = alarmSounded ? 1 : 0;
    }

    public long getLimitStartTime() {
        return _limitStartTime;
    }

    public void setLimitStartTime(long limitStartTime) {
        _limitStartTime = limitStartTime;
    }
}
