package AnomalyDetector;

import com.fasterxml.jackson.databind.PropertyNamingStrategy;
import com.fasterxml.jackson.databind.json.JsonMapper;
import com.fasterxml.jackson.datatype.jdk8.Jdk8Module;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import org.apache.flink.api.common.serialization.DeserializationSchema;
import org.apache.flink.api.common.typeinfo.TypeInformation;
import org.apache.flink.shaded.jackson2.com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;

public class TransactionDeserializationSchema implements DeserializationSchema<Transaction> {
    private static final JsonMapper objectMapper = JsonMapper.builder()
            .addModule(new Jdk8Module())
            .addModule(new JavaTimeModule())
            .build();

    @Override
    public Transaction deserialize(byte[] message) throws IOException {
        return objectMapper.readValue(message, Transaction.class);
    }

    @Override
    public boolean isEndOfStream(Transaction nextElement) {
        return false;
    }

    @Override
    public TypeInformation<Transaction> getProducedType() {
        return TypeInformation.of(Transaction.class);
    }
}
