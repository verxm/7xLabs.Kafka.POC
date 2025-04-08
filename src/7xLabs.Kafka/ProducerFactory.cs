using _7xLabs.Kafka.Managers;
using Confluent.Kafka;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka
{
    [ExcludeFromCodeCoverage]
    internal static class ProducerFactory
    {
        internal static IProducer<TKey, TValue> Create<TKey, TValue>(Action<ProducerConfig>? configureProducer = null)
        {
            var builder = new ProducerBuilder<TKey, TValue>(ProducerConfigFactory.Create(configureProducer));
            return builder
                .SetErrorHandler((producer, error) => KafkaErrorManager.Handle(error))
                .Build();
        }
    }
}
