namespace TransactionSimulator.Models;

public class Transaction
{
    public int CardId { get; set; }
    public int UserId { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public decimal Value { get; set; }
    public decimal AvailableLimit { get; set; }
}
