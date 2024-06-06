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
        if (transaction is null)
        {
            throw new ArgumentNullException("Transaction is null");
        }

        if (GetTransaction(transaction.Id) is not null)
        {
            throw new InvalidOperationException($"Transaction given by id {transaction.Id} already exists");
        }

        _transactions.Add(transaction);
        return;
    }

    public Transaction GetTransaction(int id)
    {
        var transaction = _transactions.FirstOrDefault(t => t.Id == id);

        if (transaction is null)
        {
            throw new Exception($"Transaction given by id {id} does not exist");
        }

        return transaction;
    }
}
