using _7xLabs.Kafka.Providers;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace _7xLabs.Kafka.Consumers
{
    [ExcludeFromCodeCoverage]
    internal class InstrumentedConsumer<TKey, TValue> : IConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, TValue> _kafkaConsumer;

        public InstrumentedConsumer(IConsumer<TKey, TValue> kafkaConsumer)
        {
            _kafkaConsumer = kafkaConsumer;
        }

        public string MemberId => _kafkaConsumer.MemberId;

        public List<TopicPartition> Assignment => _kafkaConsumer.Assignment;

        public List<string> Subscription => _kafkaConsumer.Subscription;

        public IConsumerGroupMetadata ConsumerGroupMetadata => _kafkaConsumer.ConsumerGroupMetadata;

        public Handle Handle => _kafkaConsumer.Handle;

        public string Name => _kafkaConsumer.Name;

        public ConsumeResult<TKey, TValue> Consume(int millisecondsTimeout)
        {
            var consumeResult = _kafkaConsumer.Consume(millisecondsTimeout);

            if (consumeResult != null)
            {
                var _ = KafkaActivityProvider.StartConsumeActivity(consumeResult);
            }

            return consumeResult!;
        }

        public ConsumeResult<TKey, TValue> Consume(CancellationToken cancellationToken = default)
        {
            var consumeResult = _kafkaConsumer.Consume(cancellationToken);

            if (consumeResult != null)
            {
                var _ = KafkaActivityProvider.StartConsumeActivity(consumeResult);
            }

            return consumeResult!;
        }

        public ConsumeResult<TKey, TValue> Consume(TimeSpan timeout)
        {
            var consumeResult = _kafkaConsumer.Consume(timeout);

            if (consumeResult != null)
            {
                var _ = KafkaActivityProvider.StartConsumeActivity(consumeResult);
            }

            return consumeResult!;
        }

        public int AddBrokers(string brokers)
            => _kafkaConsumer.AddBrokers(brokers);

        public void Assign(TopicPartition partition)
            => _kafkaConsumer.Assign(partition);

        public void Assign(TopicPartitionOffset partition)
            => _kafkaConsumer.Assign(partition);

        public void Assign(IEnumerable<TopicPartitionOffset> partitions)
            => _kafkaConsumer.Assign(partitions);

        public void Assign(IEnumerable<TopicPartition> partitions)
            => _kafkaConsumer.Assign(partitions);

        public void Close()
        {
            _kafkaConsumer.Close();
            ConsumeActivity.Close();
            Thread.Sleep(5000);
        }

        public List<TopicPartitionOffset> Commit()
        {
            var result = _kafkaConsumer.Commit();
            ConsumeActivity.Close();
            return result;
        }

        public void Commit(IEnumerable<TopicPartitionOffset> offsets)
        {
            _kafkaConsumer.Commit(offsets);
            ConsumeActivity.Close();
        }

        public void Commit(ConsumeResult<TKey, TValue> result)
        {
            _kafkaConsumer.Commit(result);
            ConsumeActivity.Close();
        }

        public List<TopicPartitionOffset> Committed(TimeSpan timeout)
            => _kafkaConsumer.Committed(timeout);

        public List<TopicPartitionOffset> Committed(IEnumerable<TopicPartition> partitions, TimeSpan timeout)
            => _kafkaConsumer.Committed(partitions, timeout);

        public void Dispose()
        {
            _kafkaConsumer.Dispose();
            ConsumeActivity.Close();
        }

        public WatermarkOffsets GetWatermarkOffsets(TopicPartition topicPartition)
            => _kafkaConsumer.GetWatermarkOffsets(topicPartition);

        public void IncrementalAssign(IEnumerable<TopicPartitionOffset> partitions)
            => _kafkaConsumer.IncrementalAssign(partitions);

        public void IncrementalAssign(IEnumerable<TopicPartition> partitions)
            => _kafkaConsumer.IncrementalAssign(partitions);

        public void IncrementalUnassign(IEnumerable<TopicPartition> partitions)
            => _kafkaConsumer?.IncrementalUnassign(partitions);

        public List<TopicPartitionOffset> OffsetsForTimes(IEnumerable<TopicPartitionTimestamp> timestampsToSearch, TimeSpan timeout)
            => _kafkaConsumer.OffsetsForTimes(timestampsToSearch, timeout);

        public void Pause(IEnumerable<TopicPartition> partitions)
            => _kafkaConsumer.Pause(partitions);

        public Offset Position(TopicPartition partition)
            => _kafkaConsumer.Position(partition);

        public TopicPartitionOffset PositionTopicPartitionOffset(TopicPartition partition)
            => _kafkaConsumer.PositionTopicPartitionOffset(partition);

        public WatermarkOffsets QueryWatermarkOffsets(TopicPartition topicPartition, TimeSpan timeout)
            => _kafkaConsumer.QueryWatermarkOffsets(topicPartition, timeout);

        public void Resume(IEnumerable<TopicPartition> partitions)
            => _kafkaConsumer.Resume(partitions);

        public void Seek(TopicPartitionOffset tpo)
            => _kafkaConsumer.Seek(tpo);

        public void SetSaslCredentials(string username, string password)
            => _kafkaConsumer.SetSaslCredentials(username, password);

        public void StoreOffset(ConsumeResult<TKey, TValue> result)
            => _kafkaConsumer.StoreOffset(result);

        public void StoreOffset(TopicPartitionOffset offset)
            => _kafkaConsumer.StoreOffset(offset);

        public void Subscribe(IEnumerable<string> topics)
            => _kafkaConsumer.Subscribe(topics);

        public void Subscribe(string topic)
            => _kafkaConsumer.Subscribe(topic);

        public void Unassign()
        {
            _kafkaConsumer.Unassign();
            ConsumeActivity.Close();
        }

        public void Unsubscribe()
        {
            _kafkaConsumer.Unsubscribe();
            ConsumeActivity.Close();
        }
    }
}
