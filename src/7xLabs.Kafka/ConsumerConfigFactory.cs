using _7xLabs.Kafka.Providers;
using Confluent.Kafka;
using System;

namespace _7xLabs.Kafka
{
    internal static class ConsumerConfigFactory
    {
        internal static ConsumerConfig Create(Action<ConsumerConfig>? configureConsumer = null)
        {
            var result = new ConsumerConfig(ClientConfigFactory.Create())
            {
                ClientId = EnvironmentVariablesProvider.MicroServiceName,
                GroupId = EnvironmentVariablesProvider.KafkaConsumerGroup,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            configureConsumer?.Invoke(result);

            return result;
        }
    }
}
