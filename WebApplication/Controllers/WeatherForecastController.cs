using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly Summary[] Summaries = Enum.GetValues<Summary>();

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(ODataQueryOptions<WeatherForecast> options)
        {
            var forecast = Enumerable.Range(1, 100).Select(index => new WeatherForecast
            {
                Key = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
            return options.ApplyTo(forecast.AsQueryable()).Cast<WeatherForecast>();
        }

        [StringAsEnumResolverFilter]
        [HttpGet("WithStringAsEnumResolver")]
        public IEnumerable<WeatherForecast> GetWithStringAsEnumResolver(ODataQueryOptions<WeatherForecast> options)
        {
            return Get(options);
        }
    }
}