using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;

namespace TransactionSimulator.Repositories.Implementations;

public class TransactionRepository : ITransactionRepository
{
    private IList<Transaction> _transactions;

    public TransactionRepository()
    {
        _transactions = new List<Transaction>();
    }

    public void AddTransaction(Transaction transaction)
    {
        if (_transactions is null)
        {
            throw new ArgumentNullException("Transaction is null");
        }

        _transactions.Add(transaction);
        return;
    }

    public IList<Transaction> GetTransactionsBuUserAndCard(int cardId, int userId)
    {
        return _transactions.Where(x => x.UserId == userId && x.CardId == cardId).ToList();
    }
}
