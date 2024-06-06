package AnomalyDetector;

import org.apache.flink.shaded.jackson2.com.fasterxml.jackson.annotation.JsonProperty;

public class Alert {

    @JsonProperty("Reason")
    private String reason;
    @JsonProperty("CardId")
    private Integer cardId;
    @JsonProperty("UserId")
    private Integer userId;
    @JsonProperty("Value")
    private double value;

    public Alert() { }

    public Alert(String reason, Integer cardId, Integer userId, double value) {
        this.reason = reason;
        this.cardId = cardId;
        this.userId = userId;
        this.value = value;
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
}
