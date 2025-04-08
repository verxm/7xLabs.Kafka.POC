using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka.Extensions.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class TagKeys
    {
        internal const string MESSAGING_SYSTEM = "messaging.system";

        internal const string DELIVERY_STATUS = "delivery.status";

        internal const string MESSAGE_KEY = "message.key";
        internal const string MESSAGE_PARTITION = "message.partition";
        internal const string MESSAGE_PARTITION_IS_SPECIAL = "message.partition.isSpecial";
        internal const string MESSAGE_OFFSET = "message.offset";
        internal const string MESSAGE_TARGET_TOPIC = "message.target.topic";
        internal const string MESSAGE_SOURCE_TOPIC = "message.source.topic";

        internal const string CORRELATION_ID = "correlation.id";
        internal const string TENANT_ID = "tenant.id";

        internal const string MACHINE_NAME = "machine.name";
        internal const string RUNTIME_FRAMEWORK = "runtime.framework";
        internal const string KERNEL = "kernel";
    }
}
