package AnomalyDetector;

import org.apache.flink.streaming.api.functions.KeyedProcessFunction;
import org.apache.flink.util.Collector;

public class MultipleTransactionsAnomaly extends KeyedProcessFunction<Integer, Transaction, Alert> {

    @Override
    public void processElement(
            Transaction transaction,
            KeyedProcessFunction<Integer, Transaction, Alert>.Context context,
            Collector<Alert> collector) {
        if (transaction.getValue() > transaction.getAvailableLimit()) {
            Alert alert = new Alert(
                    "Multiple card payments found for the user!",
                    transaction.getCardId(),
                    transaction.getUserId(),
                    transaction.getValue()
            );
            collector.collect(alert);
        }
    }
}
