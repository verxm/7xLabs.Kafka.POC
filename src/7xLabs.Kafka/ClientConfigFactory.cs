using Confluent.Kafka;

namespace _7xLabs.Kafka;

public static class ClientConfigFactory
{
    public static ClientConfig Create()
    {
        // TODO: Parameterize settings
        var clientConfig = new ClientConfig
        {
            BootstrapServers = "127.0.0.1:9092",
            SaslUsername = "",
            SaslPassword = "",
            SecurityProtocol = SecurityProtocol.SaslSsl,
            SaslMechanism = SaslMechanism.ScramSha512
        };

        return clientConfig;
    }

    static T? ParseEnum<T>(string value) where T : struct
    {
        T? enumValue = default;

        if (Enum.TryParse(value, out T parsedEnum))
        {
            enumValue = parsedEnum;
        }

        return enumValue;
    }
}
