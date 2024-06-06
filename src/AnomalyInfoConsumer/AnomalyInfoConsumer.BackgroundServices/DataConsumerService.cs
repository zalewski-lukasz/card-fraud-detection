using AnomalyInfoConsumer.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AnomalyInfoConsumer.BackgroundServices;

public class DataConsumerService : BackgroundService
{
    private readonly ILogger<DataConsumerService> _logger;
    private readonly IConsumer<Null, string> _kafkaConsumer;
    private static int _alertId = 0;

    public DataConsumerService(ILogger<DataConsumerService> logger)
    {
        _logger = logger;

        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "alerty-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _kafkaConsumer = new ConsumerBuilder<Null, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            _kafkaConsumer.Subscribe("Alerty");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _kafkaConsumer.Consume(stoppingToken);

                    if (consumeResult != null)
                    {
                        var alert = JsonConvert.DeserializeObject<Alert>(consumeResult.Message.Value);
                        alert.Id = Interlocked.Increment(ref _alertId);

                        _logger.LogInformation($"Received Alert: Id: {alert.Id}, CardId: {alert.CardId}, UserId: {alert.UserId}, Reason: {alert.Reason}, Value: {alert.Value}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Cancellation requested, stopping consumer.");
            }
            finally
            {
                _kafkaConsumer.Close();
            }
        }, stoppingToken);
    }
}
