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
package com.scaleoutsoftware.demo.simulation;

import com.scaleoutsoftware.demo.Constants;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessageProcessor;
import com.scaleoutsoftware.digitaltwin.core.DigitalTwinBase;
import com.scaleoutsoftware.digitaltwin.core.InitContext;
import com.scaleoutsoftware.digitaltwin.core.TimerActionResult;
import com.scaleoutsoftware.digitaltwin.core.TimerType;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.time.Duration;
import java.util.logging.Level;

public class DataSource extends DigitalTwinBase {
    private String nodeType;
    private String status;
    private String region;
    private double longitude;
    private double latitude;
    private boolean attacked;
    public DataSource() {

    }

    public DataSource(String model, String id, String nt, String ns, String reg, double lon, double lat, boolean atk) {
        this.Model  = model;
        this.Id     = id;
        nodeType    = nt;
        status      = ns;
        region      = reg;
        longitude   = lon;
        latitude    = lat;
        attacked    = atk;
    }

    public void applyUpdate(DataSourceMessage dsm) {
        nodeType    = dsm.getNodeType();
        status      = dsm.getStatus();
        region      = dsm.getRegion();
        longitude   = dsm.getLongitude();
        latitude    = dsm.getLatitude();
        attacked    = dsm.attack();
    }

    public String getNodeType() {
        return nodeType;
    }

    public String getRegion() {
        return region;
    }

    public double getLongitude() {
        return longitude;
    }

    public double getLatitude() {
        return latitude;
    }

    public String getStatus() {
        return Constants.convertColorToStatus(status);
    }

    public void setStatus(String s) {
        status = Constants.convertStatusToColor(s);
    }

    @Override
    public void init(InitContext context) {
        super.init(context);
        //context.startTimer("AttackTimer", Duration.ofMillis(60000), TimerType.Recurring, new AttackTimer());
    }

    public boolean attacked() {
        return attacked;
    }

    public void setAttacked(boolean atk) {
        attacked = atk;
    }

    @Override
    public String toString() {
        return "DataSource{" +
                "nodeType='" + nodeType + '\'' +
                ", status='" + status + '\'' +
                ", region='" + region + '\'' +
                ", longitude=" + longitude +
                ", latitude=" + latitude +
                ", attacked=" + attacked +
                '}';
    }
}
