using _7xLabs.Kafka.Providers;
using Confluent.Kafka;
using System;

namespace _7xLabs.Kafka
{
    internal static class ProducerConfigFactory
    {
        internal static ProducerConfig Create(Action<ProducerConfig>? configureProducer = null)
        {
            var result = new ProducerConfig(ClientConfigFactory.Create())
            {
                ClientId = EnvironmentVariablesProvider.MicroServiceName,
                Acks = Acks.All,
                EnableIdempotence = true
            };

            configureProducer?.Invoke(result);

            return result;
        }
    }
}
