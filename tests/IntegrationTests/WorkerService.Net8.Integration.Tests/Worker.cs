using _7xLabs.Kafka;
using Confluent.Kafka;
using System.Diagnostics;

namespace WorkerService.Net8.Integration.Tests;

public class Worker : BackgroundService
{
    const string SOURCE_TOPIC_NAME = "labs-integration-tests";

    private readonly ILogger<Worker> _logger;
    private readonly IConsumer<Ignore, string> _kafkaConsumer;

    public Worker(
        ILogger<Worker> logger,
        IConsumer<Ignore, string> kafkaConsumer)
    {
        _logger = logger;
        _kafkaConsumer = kafkaConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        _kafkaConsumer.Subscribe(SOURCE_TOPIC_NAME);

        var activitySource = new ActivitySource(Environment.GetEnvironmentVariable("MicroServiceName")!);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Waiting for message...");

                var consumeResult = _kafkaConsumer.Consume(stoppingToken);

                using var activity = activitySource.StartActivity("Activity after consumption.");

                //throw new Exception("Test exception");

                _kafkaConsumer.Commit(consumeResult);
                _logger.LogInformation("Consumed message: {@ConsumedMessage}", consumeResult.Message.Value);

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                ConsumeActivity.SetError(ex);
                _kafkaConsumer.Close();
                throw;
            }
        }
    }
}
