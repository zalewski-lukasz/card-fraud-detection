package AnomalyDetector;

import org.apache.flink.api.common.state.ValueState;
import org.apache.flink.api.common.state.ValueStateDescriptor;
import org.apache.flink.streaming.api.functions.KeyedProcessFunction;
import org.apache.flink.util.Collector;
import org.apache.flink.configuration.Configuration;

public class MultipleTransactionsAnomaly extends KeyedProcessFunction<Integer, Transaction, Alert> {

    // State to keep track of the number of transactions in the current window
    private transient ValueState<Integer> transactionCountState;
    // State to keep track of the timer's timestamp
    private transient ValueState<Long> timerState;

    @Override
    public void open(Configuration parameters) throws Exception {
        // Initialize the state descriptors
        ValueStateDescriptor<Integer> transactionCountDescriptor = new ValueStateDescriptor<>("transactionCount", Integer.class);
        transactionCountState = getRuntimeContext().getState(transactionCountDescriptor);

        ValueStateDescriptor<Long> timerDescriptor = new ValueStateDescriptor<>("timer", Long.class);
        timerState = getRuntimeContext().getState(timerDescriptor);
    }

    @Override
    public void processElement(
            Transaction transaction,
            KeyedProcessFunction<Integer, Transaction, Alert>.Context context,
            Collector<Alert> collector) throws Exception {

        // Get the current transaction count
        Integer currentCount = transactionCountState.value();
        if (currentCount == null) {
            currentCount = 0;
        }

        // Increment the transaction count
        currentCount += 1;
        transactionCountState.update(currentCount);

        // Get the current processing time
        long currentTime = context.timerService().currentProcessingTime();

        // Register a timer if no timer is set
        if (timerState.value() == null) {
            long timer = currentTime + 60 * 1000; // Set timer for one minute
            context.timerService().registerProcessingTimeTimer(timer);
            timerState.update(timer);
        }

        // Check if transaction count exceeds the limit
        if (currentCount > 5) {
            Alert alert = new Alert(
                    "High number of transactions detected for the user within a minute!",
                    transaction.getCardId(),
                    transaction.getUserId(),
                    transaction.getValue()
            );
            collector.collect(alert);
        }
    }

    @Override
    public void onTimer(long timestamp, KeyedProcessFunction<Integer, Transaction, Alert>.OnTimerContext ctx, Collector<Alert> out) throws Exception {
        // Reset the transaction count and clear the timer state
        transactionCountState.clear();
        timerState.clear();
    }
}
