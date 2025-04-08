using _7xLabs.Kafka.Managers;
using Confluent.Kafka;
using System;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka
{
    [ExcludeFromCodeCoverage]
    internal static class ConsumerFactory
    {
        internal static IConsumer<TMessageKey, TMessageValue> Create<TMessageKey, TMessageValue>(Action<ConsumerConfig>? configureConsumer = null)
        {
            var consumerConfig = ConsumerConfigFactory.Create(configureConsumer);

            return new ConsumerBuilder<TMessageKey, TMessageValue>(consumerConfig)
                .SetErrorHandler((consumer, error) => KafkaErrorManager.Handle(error))
                .Build();
        }
    }
}
