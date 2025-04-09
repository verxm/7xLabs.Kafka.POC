using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Api.Net8.Integration.Tests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = 
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        const string TARGET_TOPIC_NAME = "labs-integration-tests";

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IProducer<Null, string> _kafkaProducer;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IProducer<Null, string> kafkaProducer)
        {
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(result),
                Headers = new()
                {
                    {"CorrelationId", Encoding.ASCII.GetBytes("your-great-grandfather's-test") }
                }
            };

            var deliveryResult = await _kafkaProducer.ProduceAsync(TARGET_TOPIC_NAME, message);

            _logger.LogInformation("Delivery result: {@DeliveryResult}", deliveryResult);

            return result;
        }
    }
}
