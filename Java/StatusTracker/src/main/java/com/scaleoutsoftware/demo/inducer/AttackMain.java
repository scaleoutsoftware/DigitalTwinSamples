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
package com.scaleoutsoftware.demo.inducer;

import com.scaleoutsoftware.digitaltwin.datasource.AppEndpoint;
import com.scaleoutsoftware.digitaltwin.datasource.AppEndpointException;

public class AttackMain {
    public static String ATTACKER_MODEL = "Attacker";
    public static String ATTACKER_ID = "19";
    public static String[] CONSTANT_IDS = new String[] {"98072","98073","98074","98075","98082","98367","10122","10124","10125","10131","10132","10133","33109","33102","33101","33110","33111","33114"};
    public static void main(String[] args) throws AppEndpointException {
        if (args.length == 0) {
            args = CONSTANT_IDS;
        }
        AttackerMessage message = new AttackerMessage(args);
        String json = message.toJson();
        System.out.println("json: " + json);
        AppEndpoint.send(ATTACKER_MODEL, ATTACKER_ID, json);
    }

}
