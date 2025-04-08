using Confluent.Kafka;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace _7xLabs.Kafka.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class KafkaHeadersExtensions
    {
        private const string CORRELATION_ID_HEADER_KEY = "CorrelationId";
        private const string TENANT_ID_HEADER_KEY = "TenantId";

        internal static string? GetCorrelationId(this Headers headers)
        {
            return headers?
                .Get(CORRELATION_ID_HEADER_KEY)?
                .GetValueString();
        }

        internal static string? GetTenantId(this Headers headers)
        {
            return headers?
                .Get(TENANT_ID_HEADER_KEY)?
                .GetValueString();
        }

        internal static IHeader? Get(this Headers headers, string key)
        {
            return headers.FirstOrDefault(x => x.Key == key);
        }

        internal static string GetValueString(this IHeader header)
        {
            return Encoding.UTF8.GetString(header.GetValueBytes());
        }
    }
}
