package AnomalyDetector;

import java.io.Serializable;

public class Transaction implements Serializable {
    private int cardId;
    private int userId;
    private double longitude;
    private double latitude;
    private double value;
    private double availableLimit;

    public Transaction(int cardId, int userId, double longitude, double latitude, double value, double availableLimit) {
        this.cardId = cardId;
        this.userId = userId;
        this.longitude = longitude;
        this.latitude = latitude;
        this.value = value;
        this.availableLimit = availableLimit;
    }

    public int getCardId() {
        return this.cardId;
    }

    public int getUserId() {
        return this.userId;
    }

    public double getLongitude() {
        return this.longitude;
    }

    public double getLatitude() {
        return this.latitude;
    }

    public double getValue() {
        return this.value;
    }

    public double getAvailableLimit() {
        return this.availableLimit;
    }
}
