using WorkerService.Net8.Integration.Tests;
using _7xLabs.Kafka;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddKafkaConsumer(consumerConfig =>
{
    consumerConfig.MaxPollIntervalMs = 50000;
});

var host = builder.Build();
host.Run();
