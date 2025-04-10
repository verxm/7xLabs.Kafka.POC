using _7xLabs.Kafka.Consumers;
using _7xLabs.Kafka.Producers;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka;

[ExcludeFromCodeCoverage]
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection services, Action<ProducerConfig>? configureProducer = null)
    {
        return services.AddKafkaProducer<Null, string>(configureProducer);
    }

    public static IServiceCollection AddKafkaConsumer(this IServiceCollection services, Action<ConsumerConfig>? configureConsumer = null)
    {
        return services.AddKafkaConsumer<Ignore, string>(configureConsumer);
    }

    internal static IServiceCollection AddKafkaProducer<TMessageKey, TMessageValue>(this IServiceCollection services, Action<ProducerConfig>? configureProducer = null)
    {
        var kafkaProducer = ProducerFactory.Create<TMessageKey, TMessageValue>(configureProducer);
        var instrumentedKafkaProducer = new InstrumentedProducer<TMessageKey, TMessageValue>(kafkaProducer);

        return services.AddSingleton<IProducer<TMessageKey, TMessageValue>>(instrumentedKafkaProducer);
    }

    internal static IServiceCollection AddKafkaConsumer<TMessageKey, TMessageValue>(this IServiceCollection services, Action<ConsumerConfig>? configureConsumer = null)
    {
        var kafkaConsumer = ConsumerFactory.Create<TMessageKey, TMessageValue>(configureConsumer);
        var instrumentedKafkaConsumer = new InstrumentedConsumer<TMessageKey, TMessageValue>(kafkaConsumer);

        return services.AddSingleton<IConsumer<TMessageKey, TMessageValue>>(instrumentedKafkaConsumer);
    }
}
