using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka;

[ExcludeFromCodeCoverage]
public static class ConsumeActivity
{
    public static Activity? Current { get; private set; }

    public static void Set(Activity? activity)
    {
        Current = activity;
    }

    public static void Close()
    {
        Current?.Dispose();
        Current = null;
    }

    public static void SetError(Exception exception)
    {
        Current?.SetStatus(ActivityStatusCode.Error, exception.Message);
        Current?.AddException(exception);
    }
}
