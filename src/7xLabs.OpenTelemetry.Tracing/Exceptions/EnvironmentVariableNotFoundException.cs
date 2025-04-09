using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.OpenTelemetry.Tracing.Exceptions;

[ExcludeFromCodeCoverage]
internal class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException(string message) : base(message) { }

    public static void ThrowIfNullOrEmpty(string key, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EnvironmentVariableNotFoundException($"{key} can't be null or empty");
        }
    }
}
