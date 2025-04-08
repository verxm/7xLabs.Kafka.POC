using Confluent.Kafka;

namespace _7xLabs.Kafka.Managers
{
    public static class KafkaErrorManager
    {
        static readonly int MAX_KAFKA_CONNECTION_RETRY;
        static readonly long ACCEPTED_TIME_BEETWEN_CONNECTION_ERRORS = 240000;
        static int _fatalErrorsCount = 0;
        static DateTime? LastErrorOcurrence;

        const string CONNECTION_REFUSED_ERROR_MESSAGE = "Connection refused";
        const string DEFAULT_LOG_MESSAGE = "Kafka connection error handle. {FatalErrorCount} {ErrorCode} {Message} {ErrorManagerAction}";

        static KafkaErrorManager()
        {
            var maxKafkaConnectionRetryString = Environment.GetEnvironmentVariable("MAX_KAFKA_CONNECTION_RETRY");
            var isParsed = int.TryParse(maxKafkaConnectionRetryString, out MAX_KAFKA_CONNECTION_RETRY);

            if (isParsed is false)
            {
                MAX_KAFKA_CONNECTION_RETRY = 5;
            }
        }

        public static void Handle(Error error)
        {
            var reason = error.Reason;

            if (reason.Contains(CONNECTION_REFUSED_ERROR_MESSAGE) is false)
            {
                IgnoreError(error);
                return;
            }

            var errorDate = DateTime.UtcNow;
            var minimunErrorDate = errorDate.AddMilliseconds(-ACCEPTED_TIME_BEETWEN_CONNECTION_ERRORS);

            if (LastErrorOcurrence == null)
            {
                InitErrorCount(error, "Start");
                return;
            }

            var isNotAllowedTimeBetweenOcurrences = DateTime.Compare(LastErrorOcurrence.Value, minimunErrorDate) < 0;

            if (isNotAllowedTimeBetweenOcurrences)
            {
                InitErrorCount(error, "Reset");
                return;
            }

            if (_fatalErrorsCount >= MAX_KAFKA_CONNECTION_RETRY)
            {
                FinalizeEnvironment(error);
            }

            IncrementErrorCount(error);
        }

        private static void InitErrorCount(Error error, string action)
        {
            LastErrorOcurrence = DateTime.UtcNow;

            _fatalErrorsCount = 1;
            StaticLogger.LogWarning(DEFAULT_LOG_MESSAGE, _fatalErrorsCount, error.Code, error.Reason, $"{action} Counter");
        }

        private static void IncrementErrorCount(Error error)
        {
            LastErrorOcurrence = DateTime.UtcNow;
            _fatalErrorsCount++;
            StaticLogger.LogWarning(DEFAULT_LOG_MESSAGE, _fatalErrorsCount, error.Code, error.Reason, "Try Reconnect");
        }

        private static void FinalizeEnvironment(Error error)
        {
            StaticLogger.LogError(DEFAULT_LOG_MESSAGE, _fatalErrorsCount, error.Code, error.Reason, "Environment Exit");

            Task.Delay(2000)
                .GetAwaiter()
                .GetResult();

            Environment.Exit(1);
        }

        private static void IgnoreError(Error error)
        {
            StaticLogger.LogWarning(DEFAULT_LOG_MESSAGE, _fatalErrorsCount, error.Code, error.Reason, "Ignore");
        }
    }
}
