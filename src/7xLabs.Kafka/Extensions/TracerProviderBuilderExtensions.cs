using _7xLabs.Kafka.Providers;
using OpenTelemetry.Trace;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka;

[ExcludeFromCodeCoverage]
public static class TracerProviderBuilderExtensions
{
    public static TracerProviderBuilder AddKafkaInstrumentation(this TracerProviderBuilder tracerProviderBuilder)
    {
        return tracerProviderBuilder.AddSource(KafkaActivityProvider.ACTIVITY_SOURCE_NAME);
    }
}
