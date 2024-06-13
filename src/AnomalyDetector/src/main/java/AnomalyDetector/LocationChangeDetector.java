package AnomalyDetector;

import org.apache.flink.streaming.api.functions.windowing.ProcessWindowFunction;
import org.apache.flink.streaming.api.windowing.windows.TimeWindow;
import org.apache.flink.util.Collector;

public class LocationChangeDetector extends ProcessWindowFunction<Transaction, Alert, Integer, TimeWindow> {
    static final double MAX_ALLOWED_DISTANCE = 0.05;
    @Override
    public void process(Integer s,
                        ProcessWindowFunction<Transaction, Alert, Integer, TimeWindow>.Context context,
                        Iterable<Transaction> iterable, Collector<Alert> collector) {
        double latSum = 0;
        double longSum = 0;
        int count = 0;

        for (Transaction transaction : iterable) {
            latSum += transaction.getLatitude();
            longSum += transaction.getLongitude();
            count++;
        }

        double latMean = latSum / count;
        double longMean = longSum / count;

        for (Transaction transaction : iterable) {
            double distance = Math.pow(transaction.getLatitude() - latMean, 2)
                    + Math.pow(transaction.getLongitude() - longMean, 2);
            if (distance > MAX_ALLOWED_DISTANCE * MAX_ALLOWED_DISTANCE) {
                Alert alert = new Alert(
                        "Too big of a change in location in time window!",
                        transaction.getCardId(),
                        transaction.getUserId(),
                        transaction.getValue()
                );
                collector.collect(alert);
            }
        }
    }
}