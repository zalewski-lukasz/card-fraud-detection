using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;

namespace TransactionSimulator.Repositories.Implementations;

public class CardRepository : ICardRepository
{
    private IList<Card> _cards;

    public CardRepository()
    {
        _cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        if (card == null)
        {
            throw new ArgumentNullException("Provided card does not exist");
        }

        if (GetCard(card.Id) is not null)
        {
            throw new Exception($"Card given by id {card.Id} already exists");
        }

        _cards.Add(card);
        return;
    }

    public Card GetCard(int id)
    {
        var card = _cards.FirstOrDefault(t => t.Id == id);

        if (card is null)
        {
            throw new Exception($"Card given by id {id} does not exist");
        }

        return card;
    }
}
