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

        const string PRODUCE_ACTIVITY_NAME = "Confluent.Kafka.Produce";
        const string CONSUME_ACTIVITY_NAME = "Confluent.Kafka.Consume";

        static readonly ActivitySource ActivitySource = new ActivitySource(ACTIVITY_SOURCE_NAME, AssemblyData.Version);

        internal static Activity? StartProduceActivity<TKey, TValue>(
            string targetTopic,
            Message<TKey, TValue> message)
        {
            var activity = ActivitySource.StartActivity(PRODUCE_ACTIVITY_NAME, ActivityKind.Producer);

            activity?.AddProduceTags(targetTopic, message);

            return activity;
        }

        internal static Activity? StartConsumeActivity<TKey, TValue>(ConsumeResult<TKey, TValue> consumeResult)
        {
            // The current activity is set to null to ensure that the consuming activity is the root.
            Activity.Current = null;

            var activityContextFromHeaders = ExtractActivityContext(consumeResult.Message.Headers);

            var activity = ActivitySource.CreateActivity(
                CONSUME_ACTIVITY_NAME,
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
