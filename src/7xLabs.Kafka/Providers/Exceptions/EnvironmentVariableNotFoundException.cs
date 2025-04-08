using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace _7xLabs.Kafka.Providers.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal class EnvironmentVariableNotFoundException : Exception
    {
        public EnvironmentVariableNotFoundException(string key) : base(key) { }

        protected EnvironmentVariableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void ThrowIfNullOrEmpty(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new EnvironmentVariableNotFoundException(key);
            }
        }
    }
}
