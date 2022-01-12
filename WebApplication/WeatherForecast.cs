using System.ComponentModel.DataAnnotations;

namespace WebApplication
{
    public class WeatherForecast
    {
        [Key]
        public int Key { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public Summary? Summary { get; set; }
    }
}