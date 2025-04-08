using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;

public static class ConsumerExtensions
{
    public static IReadOnlyCollection<ConsumeResult<TKey, TValue>> ConsumeBatch<TKey, TValue>(
        this IConsumer<TKey, TValue> consumer,
        int maxBatchSize,
        CancellationToken cancellationToken)
    {
        var messageBatch = new List<ConsumeResult<TKey, TValue>>();
        var message = consumer.Consume(cancellationToken);

        if (message?.Message is null)
        {
            return messageBatch;
        }

        messageBatch.Add(message);

        while (messageBatch.Count < maxBatchSize)
        {
            message = consumer.Consume(TimeSpan.Zero);

            if (message?.Message is null)
            {
                break;
            }

            messageBatch.Add(message);
        }

        return messageBatch;
    }
}