namespace _7xLabs.Kafka;

public static class StaticLogger
{
    // TODO: Configure distributed Logging
    public static void LogWarning(string template, params object[] propertyValues)
    {
        Console.WriteLine(template);
    }

    public static void LogError(string template, params object[] propertyValues)
    {
        Console.WriteLine(template);
    }

    public static void LogInformation(string template, params object[] propertyValues)
    {
        Console.WriteLine(template);
    }
}
