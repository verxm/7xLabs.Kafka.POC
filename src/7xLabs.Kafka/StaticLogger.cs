namespace _7xLabs.Kafka;

public static class StaticLogger
{
    // TODO: Configure distributed Logging
    public static void LogWarning(string template, params object[] propertyValues)
    {
        Console.WriteLine(template, propertyValues);
    }

    public static void LogError(string template, params object[] propertyValues)
    {
        Console.WriteLine(template, propertyValues);
    }

    public static void LogInformation(string template, params object[] propertyValues)
    {
        Console.WriteLine(template, propertyValues);
    }
}
