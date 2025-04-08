using _7xLabs.Kafka.Extensions;
using _7xLabs.Kafka.Extensions.Constants;
using _7xLabs.Kafka.Models;
using Confluent.Kafka;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class ActivityExtensions
    {
        internal static Activity AddProduceTags<TKey, TValue>(
            this Activity activity,
            string targetTopic,
            Message<TKey, TValue> message)
        {
            activity
                .AddAssemblyTags()
                .AddMessagingSystemTag()
                .AddMessageKeyTag(message.Key)
                .AddHeaderTags(message.Headers)
                .AddTag(TagKeys.MESSAGE_TARGET_TOPIC, targetTopic);

            return activity;
        }

        internal static Activity AddConsumeTags<TKey, TValue>(
            this Activity activity,
            ConsumeResult<TKey, TValue> consumeResult)
        {
            return activity
                .AddAssemblyTags()
                .AddMessagingSystemTag()
                .AddHeaderTags(consumeResult.Message.Headers)
                .AddMessageKeyTag(consumeResult.Message.Key)
                .AddTag(TagKeys.MESSAGE_SOURCE_TOPIC, consumeResult.Topic)
                .AddTag(TagKeys.MESSAGE_OFFSET, consumeResult.Offset)
                .AddTag(TagKeys.MESSAGE_PARTITION, consumeResult.Partition.Value);
        }

        internal static void Enrich<TKey, TValue>(
            this Activity activity,
            DeliveryResult<TKey, TValue> deliveryResult)
        {
            activity?
                .AddTag(TagKeys.DELIVERY_STATUS, deliveryResult.Status)
                .AddTag(TagKeys.MESSAGE_PARTITION, deliveryResult.Partition.Value)
                .AddTag(TagKeys.MESSAGE_PARTITION_IS_SPECIAL, deliveryResult.Partition.IsSpecial)
                .AddTag(TagKeys.MESSAGE_OFFSET, deliveryResult.Offset.Value);
        }

        static Activity AddMessageKeyTag<TKey>(this Activity activity, TKey messageKey)
        {
            if (messageKey != null)
            {
                activity.AddTag(TagKeys.MESSAGE_KEY, messageKey.ToString());
            }

            return activity;
        }

        static Activity AddHeaderTags(this Activity activity, Headers headers)
        {
            var correlationId = headers.GetCorrelationId();
            if (correlationId != null)
            {
                activity.AddTag(TagKeys.CORRELATION_ID, correlationId);
            }

            var tenantId = headers.GetTenantId();
            if (tenantId != null)
            {
                activity.AddTag(TagKeys.TENANT_ID, tenantId);
            }

            return activity;
        }
        static Activity AddAssemblyTags(this Activity activity)
        {
            return activity
                .AddTag(TagKeys.MACHINE_NAME, AssemblyData.MachineName)
                .AddTag(TagKeys.RUNTIME_FRAMEWORK, AssemblyData.Framework)
                .AddTag(TagKeys.KERNEL, AssemblyData.Kernel);
        }

        static Activity AddMessagingSystemTag(this Activity activity)
        {
            return activity.AddTag(TagKeys.MESSAGING_SYSTEM, "Kafka");
        }
    }
}
