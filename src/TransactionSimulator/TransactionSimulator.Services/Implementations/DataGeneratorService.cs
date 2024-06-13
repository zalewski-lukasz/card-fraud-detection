using Bogus;
using System.Transactions;
using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;
using TransactionSimulator.Services.Interfaces;
using Transaction = TransactionSimulator.Models.Transaction;

namespace TransactionSimulator.Services.Implementations;

public class DataGeneratorService : IDataGeneratorService
{
    private readonly IUserRepository _userRepository;
    private readonly ICardRepository _cardRepository;

    public DataGeneratorService(IUserRepository userRepository,
        ICardRepository cardRepository)
    {
        _userRepository = userRepository;
        _cardRepository = cardRepository;
    }

    public void GenerateCardData(int cardCount)
    {
        var userIds = _userRepository.GetUserIds();

        var cardFaker = new Faker<Card>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.UserId, f => f.PickRandom(userIds))
            .RuleFor(c => c.CardLimit, f => f.Finance.Amount(1000, 10000))
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
            .RuleFor(t => t.CardId, f => f.PickRandom(cardIds))
            .RuleFor(t => t.UserId, (f, t) =>
            {
                var card = _cardRepository.GetCard(t.CardId);
                var user = _userRepository.GetUser(card.UserId);
                return user.Id;
            })
            .RuleFor(t => t.Longitude, f => f.Address.Longitude())
            .RuleFor(t => t.Latitude, f => f.Address.Latitude())
            .RuleFor(t => t.Value, f => f.Finance.Amount(1, 2000))
            .RuleFor(t => t.AvailableLimit, (f, t) => _cardRepository.GetCard(t.CardId).CardLimit - t.Value);

        var generatedTransactions = new List<Transaction>();

        for (int i = 0; i < transactionCount; i++)
        {
            var transaction = transactionFaker.Generate();
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

    public IList<Transaction> GenerateTransactionsForOverTheLimitAnomaly()
    {
        var cardIds = _cardRepository.GetCardIds();

        var transactionFaker = new Faker<Transaction>()
            .RuleFor(t => t.CardId, f => f.PickRandom(cardIds))
            .RuleFor(t => t.UserId, (f, t) =>
            {
                var card = _cardRepository.GetCard(t.CardId);
                var user = _userRepository.GetUser(card.UserId);
                return user.Id;
            })
            .RuleFor(t => t.Longitude, f => f.Address.Longitude())
            .RuleFor(t => t.Latitude, f => f.Address.Latitude())
            .RuleFor(t => t.Value, f => f.Finance.Amount(10000, 20000))
            .RuleFor(t => t.AvailableLimit, (f, t) => _cardRepository.GetCard(t.CardId).CardLimit - t.Value);

        var transaction = transactionFaker.Generate();
        _cardRepository.SubtractMoney(transaction.CardId, transaction.Value);

        return new List<Transaction> { transaction };
    }

    public IList<Transaction> GenerateTransactionForMultipleTransactionsAnomaly()
    {
        var cardIds = _cardRepository.GetCardIds();
        var selectedCardId = new Faker().PickRandom(cardIds);
        var card = _cardRepository.GetCard(selectedCardId);
        var user = _userRepository.GetUser(card.UserId);
        var longitude = new Faker().Address.Longitude();
        var latitude = new Faker().Address.Latitude();
        var selectedUserId = user.Id;

        var transactionFaker = new Faker<Transaction>()
            .RuleFor(t => t.CardId, _ => selectedCardId)
            .RuleFor(t => t.UserId, _ => selectedUserId)
            .RuleFor(t => t.Longitude, _ => longitude)
            .RuleFor(t => t.Latitude, _ => latitude)
            .RuleFor(t => t.Value, f => f.Finance.Amount(1, 500))
            .RuleFor(t => t.AvailableLimit, (f, t) => card.CardLimit - t.Value);

        var list = new List<Transaction>();
        var count = 10;
        for (int i = 0; i < count; i++ ) {
            var transaction = transactionFaker.Generate();
            list.Add(transaction);
            _cardRepository.SubtractMoney(transaction.CardId, transaction.Value);
        }

        return list;
    }

    public IList<Transaction> GenerateSuddenLocationChangeAnomaly()
    {
        var cardIds = _cardRepository.GetCardIds();
        var selectedCardId = new Faker().PickRandom(cardIds);
        var card = _cardRepository.GetCard(selectedCardId);
        var user = _userRepository.GetUser(card.UserId);
        var selectedUserId = user.Id;

        var transactionFaker = new Faker<Transaction>()
            .RuleFor(t => t.CardId, _ => selectedCardId)
            .RuleFor(t => t.UserId, _ => selectedUserId)
            .RuleFor(t => t.Longitude, f => f.Address.Longitude())
            .RuleFor(t => t.Latitude, f => f.Address.Latitude())
            .RuleFor(t => t.Value, f => f.Finance.Amount(1, 500))
            .RuleFor(t => t.AvailableLimit, (f, t) => card.CardLimit - t.Value);

        var list = new List<Transaction>();
        var count = 3;
        for (int i = 0; i < count; i++)
        {
            var transaction = transactionFaker.Generate();
            list.Add(transaction);
            _cardRepository.SubtractMoney(transaction.CardId, transaction.Value);
        }

        return list;
    }
}
