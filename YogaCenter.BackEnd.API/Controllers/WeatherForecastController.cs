using Microsoft.AspNetCore.Mvc;

namespace YogaCenter.BackEnd.API.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
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
        [Route("CalculatingLargestMultipleOfSum")]
        [HttpGet]
        public double GetMultiplication(int sum)
        {
            if (sum <= 1) return sum;
            if (sum <= 3) return sum - 1;
            int three = sum / 3;
            int two = sum % 3;
            if (two == 0) return (double)(Math.Pow(3, three));
            if(two == 1) return (double)(4 * Math.Pow(3, three - 1));
            return 4 * Math.Pow(3, three);
        }

        [Route("FiboTesing")]
        [HttpGet]
        public int GetFibo(int num)
        {
            if (num == 1 || num == 2) return 1;
            int i = 3;
            int one = 1, second = 1;
            int curr = 0;
            while(i <= num)
            {
                curr = one + second;
                one = second;
                second = curr;
                i++;
            }
            return curr;
        }
    }
}