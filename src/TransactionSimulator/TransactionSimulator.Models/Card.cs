namespace TransactionSimulator.Models;

public class Card
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal CardLimit { get; set; }
    public decimal MaxLimit { get; set; }
}
