using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using TransactionSimulator.Models;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.BackgroundWorkers;

public class DataProducerService : BackgroundService
{
    private readonly ILogger<DataProducerService> _logger;
    private readonly IDataGeneratorService _dataGeneratorService;
    private readonly IProducer<Null, string> _kafkaProducer;
    private Stack<string> _currentEvents;

    public DataProducerService(ILogger<DataProducerService> logger,
        IDataGeneratorService dataGeneratorService)
    {
        _logger = logger;
        _dataGeneratorService = dataGeneratorService;
        _currentEvents = new Stack<string>();

        var config = new ProducerConfig
        {
            BootstrapServers = "kafka:29092"
        };

        _kafkaProducer = new ProducerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        InitializeData();

        var random = new Random();
        while (!stoppingToken.IsCancellationRequested)
        {
            var count = random.Next(1, 3);
            _logger.LogInformation($"Producing new {count} transactions at time: {DateTimeOffset.Now}");
            GenerateTransactions(count);
            await Task.Delay(1000, stoppingToken);
        }
    }

    public void RegisterAnomalyEvent(string eventName)
    {
        _currentEvents.Push(eventName);
    }

    private void InitializeData()
    {
        _dataGeneratorService.GenerateUserData(2000);
        _dataGeneratorService.GenerateCardData(10000);
    }

    private void GenerateTransactions(int count)
    {
        var transactions = _dataGeneratorService.GenerateTransactionData(count);
        transactions.Concat(GenerateAnomalyTransactions());

        foreach(var transaction in transactions)
        {
            _logger.LogInformation($"Generated new transaction: userId: {transaction.UserId}, cardId: {transaction.CardId}, value: {transaction.Value}");

            string jsonString = $"{{\"userId\": {transaction.UserId}, \"cardId\": {transaction.CardId}, \"value\": {transaction.Value.ToString(CultureInfo.InvariantCulture)}, \"longitude\": {transaction.Longitude.ToString(CultureInfo.InvariantCulture)}, \"latitude\": {transaction.Latitude.ToString(CultureInfo.InvariantCulture)}, \"availableLimit\": {transaction.AvailableLimit.ToString(CultureInfo.InvariantCulture)}}}";
            var message = new Message<Null, string>
            {
                Value = jsonString
            };

            _kafkaProducer.Produce("Transakcje", message, deliveryReport =>
            {
                if (deliveryReport.Error.IsError)
                {
                    _logger.LogError($"Failed to deliver message: {deliveryReport.Error.Reason}");
                }
                else
                {
                    _logger.LogInformation($"Message delivered to {deliveryReport.TopicPartitionOffset}");
                }
            });
        }

        _kafkaProducer.Flush(TimeSpan.FromSeconds(1));
    }

    private IList<Transaction> GenerateAnomalyTransactions()
    {
        var list = new List<Transaction>();

        if (_currentEvents.Count > 0)
        {
            return list;
        }

        while ( _currentEvents.Count > 0)
        {
            var anomalyEvent = _currentEvents.Pop();

            switch (anomalyEvent)
            {
                case "OVER_THE_LIMIT_ANOMALY": 
                    list.Concat(_dataGeneratorService.GenerateTransactionsForOverTheLimitAnomaly());
                    break;
                case "MULTIPLE_TRANSACTIONS_ANOMALY":
                    list.Concat(_dataGeneratorService.GenerateTransactionForMultipleTransactionsAnomaly());
                    break;
                default:
                    break;
            }
        }

        return list;
    }
}
