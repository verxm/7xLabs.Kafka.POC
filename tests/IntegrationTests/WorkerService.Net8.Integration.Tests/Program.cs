using WorkerService.Net8.Integration.Tests;
using _7xLabs.Kafka;
using _7xLabs.OpenTelemetry.Tracing;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var _ = OpenTelemetryTracingExtensions.CreateTracerProvider(builder =>
{
    builder
        .AddKafkaInstrumentation()
        .AddSource(Environment.GetEnvironmentVariable("MicroServiceName")!);
});

builder.Services.AddKafkaConsumer(consumerConfig =>
{
    consumerConfig.MaxPollIntervalMs = 50000;
});

var host = builder.Build();
host.Run();
