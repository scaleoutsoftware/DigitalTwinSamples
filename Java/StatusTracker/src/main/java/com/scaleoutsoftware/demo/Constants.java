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
package com.scaleoutsoftware.demo;

public class Constants {
    public static String REALTIME_MODEL_NAME        = "StatusTracker";
    public static String NODE_TYPE_INFRASTRUCTURE   = "infrastructure";
    public static String NODE_TYPE_CONTROLLER       = "controller";

    /* Power grid node status */
    public static String MESSAGE_TYPE_INIT          = "init";       // initialization message
    public static String MESSAGE_TYPE_STATUS        = "status";     // status message
    public static String NODE_CONDITION_OFFLINE     = "offline";    // node was powered off
    public static String NODE_CONDITION_NORMAL      = "normal";     // base condition
    public static String NODE_CONDITION_MINOR       = "minor";      // previously abnormal
    public static String NODE_CONDITION_MODERATE    = "moderate";   // previously suspect attack
    public static String NODE_CONDITION_SEVERE      = "severe";     // previously attack

    /* Power Grid node location */
    public static String NODE_REGION_NW = "NW";
    public static String NODE_REGION_SW = "SW";
    public static String NODE_REGION_MN = "MN";
    public static String NODE_REGION_MS = "MS";
    public static String NODE_REGION_NE = "NE";
    public static String NODE_REGION_SE = "SE";
    public static long SIMULATION_DELAY_OFFLINE = 20000L;
    public static long SIMULATION_DELAY_NORMAL = 6000L;
    public static long SIMULATION_DELAY_MINOR = 6000L;
    public static long SIMULATION_DELAY_MODERATE = 10000L;
    public static long SIMULATION_DELAY_SEVERE = 60000L;

    public static int SIMULATION_PR_OFFLINE	    = 5;
    public static int SIMULATION_PR_NORMAL	    = 80;
    public static int SIMULATION_PR_MINOR	    = 20;
    public static int SIMULATION_PR_MODERATE    = 5;
    public static int SIMULATION_PR_SEVERE		= 1;

    /* Alert Levels for Controller and Infrastructure power grid nodes. */
    // controller and infrastructure normal alert levels
    public static int CONTROLLER_NORMAL_ALERTLEVEL          = 0;
    public static int INFRASTRUCTURE_NORMAL_ALERTLEVEL      = 0;

    // controller and infrastructure minor alert levels
    public static int CONTROLLER_MINOR_ALERT_LEVEL          = 2;
    public static int INFRASTRUCTURE_MINOR_ALERTLEVEL       = 1;

    // controller and infrastructure moderate alert levels
    public static int CONTROLLER_MODERATE_ALERTLEVEL        = 8;
    public static int INFRASTRUCTURE_MODERATE_ALERTLEVEL    = 4;

    // controller and infrastructure severe alert levels
    public static int CONTROLLER_SEVERE_ALERTLEVEL          = 20;
    public static int INFRASTRUCTURE_SEVERE_ALERTLEVEL      = 10;

    /* Prevent power grid state object memory growth */
    public static int MAX_INCIDENT_LIST_SIZE        = 15;
    public static int INCIDENT_LIST_SUBLIST_FROM    = 10;
    public static int INCIDENT_LIST_SUBLIST_TO      = MAX_INCIDENT_LIST_SIZE;

    public static String convertColorToStatus(String color) {
        switch(color) {
            case "blue":
                return Constants.NODE_CONDITION_OFFLINE;
            case "green":
                return Constants.NODE_CONDITION_NORMAL;
            case "yellow":
                return Constants.NODE_CONDITION_MINOR;
            case "orange":
                return Constants.NODE_CONDITION_MODERATE;
            case "red":
                return Constants.NODE_CONDITION_SEVERE;
            default:
                throw new IllegalStateException("Unknown status: " + color);
        }
    }

    public static String convertStatusToColor(String status) {
        switch(status) {
            case "offline":
                return "blue";
            case "normal":
                return "green";
            case "minor":
                return "yellow";
            case "moderate":
                return "orange";
            case "severe":
                return "red";
            default:
                throw new IllegalStateException("Unknown status: " + status);
        }

    }

}
