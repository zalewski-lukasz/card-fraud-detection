using TransactionSimulator.Models;

namespace TransactionSimulator.Repositories.Interfaces;

public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    IList<Transaction> GetTransactionsBuUserAndCard(int cardId, int userId);
}
