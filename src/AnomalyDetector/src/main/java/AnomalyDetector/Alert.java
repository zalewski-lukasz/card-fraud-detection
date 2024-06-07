package AnomalyDetector;

import org.apache.flink.shaded.jackson2.com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonFormat;

import java.time.LocalDateTime;

public class Alert {

    @JsonProperty("Reason")
    private String reason;
    @JsonProperty("CardId")
    private Integer cardId;
    @JsonProperty("UserId")
    private Integer userId;
    @JsonProperty("Value")
    private double value;
    @JsonProperty("Timestamp")
    @JsonFormat(pattern = "MM/dd/yyyy HH:mm:ss")
    private LocalDateTime timestamp;

    public Alert() { }

    public Alert(String reason, Integer cardId, Integer userId, double value) {
        this.reason = reason;
        this.cardId = cardId;
        this.userId = userId;
        this.value = value;
        this.timestamp = LocalDateTime.now();
    }

    public String getReason() {
        return this.reason;
    }

    public Integer getCardId() {
        return this.cardId;
    }

    public Integer getUserId() {
        return this.userId;
    }

    public double getValue() {
        return this.value;
    }

    public LocalDateTime getTimestamp() { return this.timestamp; }
}
