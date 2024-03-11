package com.scaleoutsoftware.demo.inducer;

import com.scaleoutsoftware.demo.Constants;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessage;
import com.scaleoutsoftware.demo.realtime.StatusTrackerMessageBuilder;
import com.scaleoutsoftware.demo.simulation.DataSourceMessage;
import com.scaleoutsoftware.digitaltwin.core.MessageProcessor;
import com.scaleoutsoftware.digitaltwin.core.ProcessingContext;
import com.scaleoutsoftware.digitaltwin.core.ProcessingResult;

public class AttackerMessageProcessor extends MessageProcessor<Attacker,AttackerMessage> {
    @Override
    public ProcessingResult processMessages(ProcessingContext processingContext, Attacker attacker, Iterable<AttackerMessage> messages) throws Exception {
        for(AttackerMessage msg : messages) {
            for(String id : msg.ids) {
                StatusTrackerMessage statusTrackerMessage = new StatusTrackerMessageBuilder(processingContext.getDataSourceId())
                        .setMessageType(Constants.MESSAGE_TYPE_STATUS)
                        .setNodeCondition(Constants.NODE_CONDITION_SEVERE)
                        .build();
                processingContext.sendToDigitalTwin("StatusTracker", id, statusTrackerMessage);
                DataSourceMessage dataSourceMessage = new DataSourceMessage(true);
                processingContext.sendToDigitalTwin("DataSource", id, dataSourceMessage);
            }
        }
        return ProcessingResult.UpdateDigitalTwin;
    }
}
