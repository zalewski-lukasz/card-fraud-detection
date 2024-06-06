using TransactionSimulator.Models;

namespace TransactionSimulator.Repositories.Interfaces;

public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    Transaction GetTransaction(int id);
}
