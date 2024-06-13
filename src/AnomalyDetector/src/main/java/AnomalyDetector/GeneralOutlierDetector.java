package AnomalyDetector;

import org.apache.flink.api.common.state.ValueState;
import org.apache.flink.api.common.state.ValueStateDescriptor;
import org.apache.flink.configuration.Configuration;
import org.apache.flink.streaming.api.functions.KeyedProcessFunction;
import org.apache.flink.util.Collector;
import org.apache.commons.math3.stat.descriptive.DescriptiveStatistics;

public class GeneralOutlierDetector extends KeyedProcessFunction<Integer, Transaction, Alert> {

    private transient ValueState<DescriptiveStatistics> statsState;
    private static final double THRESHOLD_MULTIPLIER = 3.0;  // Threshold for identifying outliers

    @Override
    public void open(Configuration parameters) throws Exception {
        ValueStateDescriptor<DescriptiveStatistics> descriptor = new ValueStateDescriptor<>(
                "statsState",
                DescriptiveStatistics.class
        );
        statsState = getRuntimeContext().getState(descriptor);
    }

    @Override
    public void processElement(
            Transaction transaction,
            KeyedProcessFunction<Integer, Transaction, Alert>.Context context,
            Collector<Alert> collector) throws Exception {

        DescriptiveStatistics stats = statsState.value();
        if (stats == null) {
            stats = new DescriptiveStatistics();
        }

        double value = transaction.getValue();

        // Check if the transaction is an outlier
        double mean = stats.getMean();
        double stdDev = stats.getStandardDeviation();
        if (stats.getN() > 0 && (Math.abs(value - mean) > THRESHOLD_MULTIPLIER * stdDev)) {
            Alert alert = new Alert(
                    "General outlier transaction detected!",
                    transaction.getCardId(),
                    transaction.getUserId(),
                    transaction.getValue()
            );
            collector.collect(alert);
        }

        // Update the statistics with the new value
        stats.addValue(value);
        statsState.update(stats);
    }
}
