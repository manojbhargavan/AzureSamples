using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SBWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task PostToQueue(WeatherForecast forecast)
        {
            // SB Client
            ServiceBusClient serviceBusClient = new(_configuration.GetConnectionString("ServiceBus"));
            var messageSender = serviceBusClient.CreateSender("WeatherForecastQ");
            var body = JsonConvert.SerializeObject(forecast);
            var message = new ServiceBusMessage(body);
            if (body.ToLower().Contains("sch"))
            {
                message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddSeconds(30);
            }

            if(body.ToLower().Contains("ttl"))
            {
                message.TimeToLive = TimeSpan.FromSeconds(20);
            }
            

            await messageSender.SendMessageAsync(message);
        }
    }
}