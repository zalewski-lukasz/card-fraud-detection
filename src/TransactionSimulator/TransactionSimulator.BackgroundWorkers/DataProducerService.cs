using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.BackgroundWorkers;

public class DataProducerService : BackgroundService
{
    private readonly ILogger<DataProducerService> _logger;
    private readonly IDataGeneratorService _dataGeneratorService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IProducer<Null, string> _kafkaProducer;

    public DataProducerService(ILogger<DataProducerService> logger,
        IDataGeneratorService dataGeneratorService,
        ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _dataGeneratorService = dataGeneratorService;
        _transactionRepository = transactionRepository;

        CreateTopicAsync("kafka:29092", "Transakcje").ConfigureAwait(true);
        CreateTopicAsync("kafka:29092", "Alerty").ConfigureAwait(true);

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
            var anomalyChance = random.Next(1, 100);

            if (anomalyChance == 1)
            {
                _logger.LogInformation($"Randomly generating anomaly data for multiple transactions anomaly...");
                SendTransactionData(_dataGeneratorService.GenerateTransactionForMultipleTransactionsAnomaly());
            }

            if (anomalyChance == 2)
            {
                _logger.LogInformation($"Randomly generating anomaly data for sudden location change anomaly...");
                SendTransactionData(_dataGeneratorService.GenerateSuddenLocationChangeAnomaly());
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private void InitializeData()
    {
        _dataGeneratorService.GenerateUserData(2000);
        _dataGeneratorService.GenerateCardData(10000);
    }

    private void SendTransactionData(IList<Transaction> transactions)
    {
        if (transactions is null || transactions.Count == 0) return;

        foreach (var transaction in transactions)
        {
            _logger.LogInformation($"Generated new transaction: userId: {transaction.UserId}, cardId: {transaction.CardId}, value: {transaction.Value}");
            _transactionRepository.AddTransaction(transaction);

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
    }

    private void GenerateTransactions(int count)
    {
        var transactions = _dataGeneratorService.GenerateTransactionData(count);

        foreach (var transaction in transactions)
        {
            _logger.LogInformation($"Generated new transaction: userId: {transaction.UserId}, cardId: {transaction.CardId}, value: {transaction.Value}");
            _transactionRepository.AddTransaction(transaction);

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

        _kafkaProducer.Flush(TimeSpan.FromSeconds(3));
    }

    public void GenerateAnomalyTransactions(string anomalyEvent)
    {
        _logger.LogInformation($"New anomaly of type {anomalyEvent} is being generated...");

        switch (anomalyEvent)
        {
            case "OVER_THE_LIMIT_ANOMALY":
                SendTransactionData(_dataGeneratorService.GenerateTransactionsForOverTheLimitAnomaly());
                break;
            case "MULTIPLE_TRANSACTIONS_ANOMALY":
                SendTransactionData(_dataGeneratorService.GenerateTransactionForMultipleTransactionsAnomaly());
                break;
            case "LOCATION_ANOMALY":
                SendTransactionData(_dataGeneratorService.GenerateSuddenLocationChangeAnomaly());
                break;
            default:
                break;
        }
    }

    static async Task CreateTopicAsync(string bootstrapServers, string topicName)
    {
        using var adminClient = new AdminClientBuilder(new AdminClientConfig
        {
            BootstrapServers = bootstrapServers
        }).Build();

        try
        {
            await adminClient.CreateTopicsAsync(new TopicSpecification[]
            {
            new TopicSpecification
            {
                Name = topicName,
                ReplicationFactor = 1,
                NumPartitions = 1
            }
            });
        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
        }
    }
}
