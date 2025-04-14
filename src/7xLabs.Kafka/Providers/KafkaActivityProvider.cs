using _7xLabs.Kafka.Extensions;
using _7xLabs.Kafka.Models;
using Confluent.Kafka;
using OpenTelemetry;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace _7xLabs.Kafka.Providers
{
    [ExcludeFromCodeCoverage]
    internal static class KafkaActivityProvider
    {
        internal const string ACTIVITY_SOURCE_NAME = "DirectOne.RogueOne.Kafka.Diagnostics";

        const string PRODUCE_ACTIVITY_NAME_TEMPLATE = "Kafka.Produce ({0})";
        const string CONSUME_ACTIVITY_NAME_TEMPLATE = "Kafka.Consume ({0})";

        static readonly ActivitySource ActivitySource = new ActivitySource(ACTIVITY_SOURCE_NAME, AssemblyData.Version);

        internal static Activity? StartProduceActivity<TKey, TValue>(
            string targetTopic,
            Message<TKey, TValue> message)
        {
            var activityName = string.Format(PRODUCE_ACTIVITY_NAME_TEMPLATE, targetTopic);

            var activity = ActivitySource.StartActivity(activityName, ActivityKind.Producer);

            activity?.AddProduceTags(targetTopic, message);

            return activity;
        }

        internal static Activity? StartConsumeActivity<TKey, TValue>(ConsumeResult<TKey, TValue> consumeResult)
        {
            // The current activity is set to null to ensure that the consuming activity is the root.
            Activity.Current = null;

            var activityContextFromHeaders = ExtractActivityContext(consumeResult.Message.Headers);

            var activityName = string.Format(CONSUME_ACTIVITY_NAME_TEMPLATE, consumeResult.Topic);

            var activity = ActivitySource.CreateActivity( // TODO: Understand why the second service does not appear in the serviceMap.
                activityName,
                ActivityKind.Consumer,
                activityContextFromHeaders);

            activity?.AddConsumeTags(consumeResult);

            ConsumeActivity.Set(activity);

            return activity?.Start();
        }

        static ActivityContext ExtractActivityContext(Headers headers)
        {
            var parentContext = OtelPropagatorProvider
                .Propagator
                .Extract(default, headers, ExtractTraceContextFromHeaders);

            Baggage.Current = parentContext.Baggage;

            return parentContext.ActivityContext;
        }

        static IEnumerable<string> ExtractTraceContextFromHeaders(Headers headers, string key)
        {
            headers ??= new Headers();

            var header = headers.FirstOrDefault(h => h.Key == key);
            if (header != null)
            {
                return new[] { Encoding.UTF8.GetString(header.GetValueBytes()) };
            }

            return Enumerable.Empty<string>();
        }
    }
}
