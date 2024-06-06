namespace TransactionSimulator.Models;

public class Transaction
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public int UserId { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public float Value { get; set; }
    public float AvailableLimit { get; set; }
}
