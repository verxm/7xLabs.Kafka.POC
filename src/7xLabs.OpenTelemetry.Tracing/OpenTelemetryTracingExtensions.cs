using _7xLabs.OpenTelemetry.Tracing.Exceptions;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace _7xLabs.OpenTelemetry.Tracing;

public static class OpenTelemetryTracingExtensions
{
    const string SERVICE_NAME_ENV_VARIABLE_KEY = "MicroServiceName";
    const string OPEN_TELEMETRY_COLLECTOR_URL_VARIABLE_KEY = "OpenTelemetryCollectorUrl";

    static string? _serviceName;
    static string? _openTelemetryCollectorUrl;

    public static TracerProvider? CreateTracerProvider(
        Action<TracerProviderBuilder>? tracerProviderOptions = null,
        bool exportConsole = false,
        bool useHttpInstrumentations = false)
    {
        SetupParameters();

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(_serviceName!);

        var tracerProviderBuilder = Sdk
            .CreateTracerProviderBuilder()
            .SetErrorStatusOnException()
            .SetResourceBuilder(resourceBuilder)
            .AddSource(_serviceName!)
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(_openTelemetryCollectorUrl!);
            });

        tracerProviderOptions?.Invoke(tracerProviderBuilder);

        if (useHttpInstrumentations)
        {
            tracerProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
        }

        if (exportConsole)
        {
            tracerProviderBuilder.AddConsoleExporter();
        }

        return tracerProviderBuilder.Build();
    }

    private static void SetupParameters()
    {
        _serviceName = Environment.GetEnvironmentVariable(SERVICE_NAME_ENV_VARIABLE_KEY);
        EnvironmentVariableNotFoundException.ThrowIfNullOrEmpty(SERVICE_NAME_ENV_VARIABLE_KEY, _serviceName);

        _openTelemetryCollectorUrl = Environment.GetEnvironmentVariable(OPEN_TELEMETRY_COLLECTOR_URL_VARIABLE_KEY);
        EnvironmentVariableNotFoundException.ThrowIfNullOrEmpty(OPEN_TELEMETRY_COLLECTOR_URL_VARIABLE_KEY, _openTelemetryCollectorUrl);
    }
}
