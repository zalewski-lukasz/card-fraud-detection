namespace AnomalyInfoConsumer.Models;

public class Alert
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public int UserId { get; set; }
    public required string Reason { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp {  get; set; }
}
