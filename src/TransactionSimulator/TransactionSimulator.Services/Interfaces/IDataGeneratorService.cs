using TransactionSimulator.Models;

namespace TransactionSimulator.Services.Interfaces;

public interface IDataGeneratorService
{
    void GenerateUserData(int userCount);
    void GenerateCardData(int cardCount);
    IList<Transaction> GenerateTransactionData(int transactionCount);
    IList<Transaction> GenerateTransactionsForOverTheLimitAnomaly();
    IList<Transaction> GenerateTransactionForMultipleTransactionsAnomaly();
}
