using TransactionSimulator.Models;

namespace TransactionSimulator.Repositories.Interfaces;

public interface ICardRepository
{
    void AddCard(Card card);
    Card GetCard(int id);
}
