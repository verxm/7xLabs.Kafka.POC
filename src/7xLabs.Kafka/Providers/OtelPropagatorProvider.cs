using OpenTelemetry.Context.Propagation;
using System.Diagnostics.CodeAnalysis;

namespace _7xLabs.Kafka.Providers;

[ExcludeFromCodeCoverage]
internal static class OtelPropagatorProvider
{
    internal static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;
}
