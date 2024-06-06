namespace TransactionSimulator.Models;

public class Card
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public float CardLimit { get; set; }
    public float MaxLimit { get; set; }
}
