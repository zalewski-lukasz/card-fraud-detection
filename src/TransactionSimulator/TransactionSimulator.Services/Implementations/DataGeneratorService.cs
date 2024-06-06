using Bogus;
using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.Services.Implementations;

public class DataGeneratorService : IDataGeneratorService
{
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICardRepository _cardRepository;

    public DataGeneratorService(IUserRepository userRepository,
        ITransactionRepository transactionRepository, 
        ICardRepository cardRepository)
    {
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
        _cardRepository = cardRepository;
    }

    public void GenerateCardData(int cardCount)
    {
        var userIds = _userRepository.GetUserIds();

        var cardFaker = new Faker<Card>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.UserId, f => f.PickRandom(userIds))
            .RuleFor(c => c.CardLimit, f => (float)f.Finance.Amount(1000, 10000))
            .RuleFor(c => c.MaxLimit, (f, t) => t.CardLimit);

        for (int i = 0; i < cardCount; i++)
        {
            _cardRepository.AddCard(cardFaker.Generate());
        }
    }

    public IList<Transaction> GenerateTransactionData(int transactionCount)
    {
        var cardIds = _cardRepository.GetCardIds();

        var transactionFaker = new Faker<Transaction>()
            .RuleFor(t => t.Id, f => f.IndexFaker + 1)
            .RuleFor(t => t.CardId, f => f.PickRandom(cardIds))
            .RuleFor(t => t.UserId, (f, t) => _userRepository.GetUser(t.UserId).Id)
            .RuleFor(t => t.Longitude, f => f.Address.Longitude())
            .RuleFor(t => t.Latitude, f => f.Address.Latitude())
            .RuleFor(t => t.Value, f => (float)f.Finance.Amount(1, 2000))
            .RuleFor(t => t.AvailableLimit, (f, t) => _cardRepository.GetCard(t.CardId).CardLimit - t.Value);

        var generatedTransactions = new List<Transaction>();

        for (int i = 0; i < transactionCount; i++)
        {
            var transaction = transactionFaker.Generate();
            _transactionRepository.AddTransaction(transaction);
            _cardRepository.SubtractMoney(transaction.CardId, transaction.Value);
            generatedTransactions.Add(transaction);
        }

        return generatedTransactions;
    }

    public void GenerateUserData(int userCount)
    {
        var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Surname, f => f.Name.LastName());

        for (int i = 0; i < userCount; i++)
        {
            _userRepository.AddUser(userFaker.Generate());
        }
    }
}
