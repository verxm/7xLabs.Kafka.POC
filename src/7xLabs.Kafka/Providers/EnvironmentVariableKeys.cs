using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka.Providers
{
    [ExcludeFromCodeCoverage]
    internal static class EnvironmentVariableKeys
    {
        internal const string KAFKA_CONNECTION_STRING = "KafkaConnectionString";
        internal const string KAFKA_CONSUMER_GROUP = "KafkaConsumerGroup";
        internal const string MICRO_SERVICE_NAME = "MicroServiceName";
    }
}
