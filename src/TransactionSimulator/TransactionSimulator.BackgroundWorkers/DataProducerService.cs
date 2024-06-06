using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.BackgroundWorkers;

public class DataProducerService : BackgroundService
{
    private readonly ILogger<DataProducerService> _logger;
    private readonly IDataGeneratorService _dataGeneratorService;
    private readonly IProducer<Null, string> _kafkaProducer;

    public DataProducerService(ILogger<DataProducerService> logger,
        IDataGeneratorService dataGeneratorService)
    {
        _logger = logger;
        _dataGeneratorService = dataGeneratorService;

        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        _kafkaProducer = new ProducerBuilder<Null, string>(config).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        InitializeData();

        var random = new Random();
        while (!stoppingToken.IsCancellationRequested)
        {
            var count = random.Next(1, 10);
            _logger.LogInformation($"Producing new {count} transactions at time: {DateTimeOffset.Now}");
            GenerateTransactions(random.Next(1, 10));
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void InitializeData()
    {
        _dataGeneratorService.GenerateUserData(2000);
        _dataGeneratorService.GenerateCardData(10000);
    }

    private void GenerateTransactions(int count)
    {
        var transactions = _dataGeneratorService.GenerateTransactionData(count);
        foreach(var transaction in transactions)
        {
            _logger.LogInformation($"Generated new transaction: userId: {transaction.UserId}, cardId: {transaction.CardId}, value: {transaction.Value}");

            var message = new Message<Null, string>
            {
                Value = $"{{\"UserId\": {transaction.UserId}, \"CardId\": {transaction.CardId}, \"Value\": {transaction.Value}, \"Longitude\": {transaction.Longitude}, \"Latitude\": {transaction.Latitude}, \"AvailableLimit\": {transaction.AvailableLimit}}}"
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
}
