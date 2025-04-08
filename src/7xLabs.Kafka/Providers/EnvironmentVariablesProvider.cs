using _7xLabs.Kafka.Providers.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka.Providers
{
    [ExcludeFromCodeCoverage]
    internal static class EnvironmentVariablesProvider
    {
        internal static string MicroServiceName
        {
            get => GetRequiredVariable(EnvironmentVariableKeys.MICRO_SERVICE_NAME);
        }

        internal static string KafkaConsumerGroup
        {
            get => GetRequiredVariable(EnvironmentVariableKeys.KAFKA_CONSUMER_GROUP);
        }

        static string GetRequiredVariable(string variableKey)
        {
            var variableValue = Environment.GetEnvironmentVariable(variableKey);
            EnvironmentVariableNotFoundException.ThrowIfNullOrEmpty(variableKey, variableValue);

            return variableValue!;
        }
    }
}
