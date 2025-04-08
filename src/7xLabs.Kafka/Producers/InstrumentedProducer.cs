using _7xLabs.Kafka.Extensions;
using _7xLabs.Kafka.Providers;
using Confluent.Kafka;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _7xLabs.Kafka.Producers
{
    [ExcludeFromCodeCoverage]
    internal class InstrumentedProducer<TKey, TValue> : IProducer<TKey, TValue>
    {
        private readonly IProducer<TKey, TValue> _kafkaProducer;

        public InstrumentedProducer(IProducer<TKey, TValue> producer)
        {
            _kafkaProducer = producer;
        }

        public Handle Handle => _kafkaProducer.Handle;

        public string Name => _kafkaProducer.Name;

        public void Produce(
            string topic,
            Message<TKey, TValue> message,
            Action<DeliveryReport<TKey, TValue>> deliveryHandler = null!)
        {
            using Activity? activity = CreateActivity(topic, message);

            _kafkaProducer.Produce(topic, message, deliveryHandler);
        }

        public void Produce(
            TopicPartition topicPartition,
            Message<TKey, TValue> message,
            Action<DeliveryReport<TKey, TValue>> deliveryHandler = null!)
        {
            using Activity? activity = CreateActivity(topicPartition.Topic, message);

            _kafkaProducer.Produce(topicPartition, message, deliveryHandler);
        }

        public async Task<DeliveryResult<TKey, TValue>> ProduceAsync(
            string topic,
            Message<TKey, TValue> message,
            CancellationToken cancellationToken = default)
        {
            using Activity? activity = CreateActivity(topic, message);

            var deliveryResult = await _kafkaProducer.ProduceAsync(topic, message, cancellationToken);

            activity?.Enrich(deliveryResult);

            return deliveryResult;
        }

        public async Task<DeliveryResult<TKey, TValue>> ProduceAsync(
            TopicPartition topicPartition,
            Message<TKey, TValue> message,
            CancellationToken cancellationToken = default)
        {
            using Activity? activity = CreateActivity(topicPartition.Topic, message);

            var deliveryResult = await _kafkaProducer.ProduceAsync(topicPartition, message, cancellationToken);

            activity?.Enrich(deliveryResult);

            return deliveryResult;
        }

        public void AbortTransaction(TimeSpan timeout)
            => _kafkaProducer.AbortTransaction(timeout);

        public void AbortTransaction()
            => _kafkaProducer.AbortTransaction();

        public int AddBrokers(string brokers)
            => _kafkaProducer.AddBrokers(brokers);

        public void BeginTransaction()
            => _kafkaProducer.BeginTransaction();

        public void CommitTransaction(TimeSpan timeout)
            => _kafkaProducer.CommitTransaction(timeout);

        public void CommitTransaction()
            => _kafkaProducer.CommitTransaction();

        public void Dispose()
            => _kafkaProducer.Dispose();

        public int Flush(TimeSpan timeout)
            => _kafkaProducer.Flush(timeout);

        public void Flush(CancellationToken cancellationToken = default)
            => _kafkaProducer.Flush();

        public void InitTransactions(TimeSpan timeout)
            => _kafkaProducer.InitTransactions(timeout);

        public int Poll(TimeSpan timeout)
            => _kafkaProducer.Poll(timeout);

        public void SendOffsetsToTransaction(IEnumerable<TopicPartitionOffset> offsets, IConsumerGroupMetadata groupMetadata, TimeSpan timeout)
            => _kafkaProducer.SendOffsetsToTransaction(offsets, groupMetadata, timeout);

        public void SetSaslCredentials(string username, string password)
            => _kafkaProducer.SetSaslCredentials(username, password);

        private Activity? CreateActivity(string topic, Message<TKey, TValue> message)
        {
            var activity = KafkaActivityProvider.StartProduceActivity(topic, message);
            if (activity != null)
            {
                OtelPropagatorProvider
                    .Propagator
                    .Inject(
                        new PropagationContext(activity.Context, Baggage.Current),
                        message.Headers,
                        InjectTraceContextIntoMessageHeaders);
            }

            return activity;
        }

        private void InjectTraceContextIntoMessageHeaders(Headers headers, string key, string value)
        {
            headers ??= new Headers();
            if (headers.Any(x => x.Key == key))
            {
                headers.Remove(key);
            }

            var header = new Header(key, Encoding.UTF8.GetBytes(value));
            headers.Add(header);
        }
    }
}
