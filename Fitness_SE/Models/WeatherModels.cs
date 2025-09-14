using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FitnessAppAPI.Models
{
    /// <summary>
    /// Represents weather forecast data with temperature and conditions
    /// </summary>
    public class WeatherForecast
    {
        [Required]
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        [Range(-50, 60)]
        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [StringLength(50)]
        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("windSpeed")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("pressure")]
        public double Pressure { get; set; }

        public WeatherForecast()
        {
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

        public WeatherForecast(DateOnly date, int temperatureC, string? summary) : this()
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }
    }

    /// <summary>
    /// Weather severity enumeration
    /// </summary>
    public enum WeatherSeverity
    {
        Mild,
        Moderate,
        Harsh,
        Extreme
    }

    /// <summary>
    /// Extended weather information
    /// </summary>
    public class ExtendedWeatherInfo : WeatherForecast
    {
        public int UvIndex { get; set; }
        public double Visibility { get; set; }
        public int CloudCover { get; set; }

        public bool IsIdealForFitness() => true;
        public string GetRecommendedWorkoutType() => "Good for workout";
    }

    /// <summary>
    /// Weather alert information
    /// </summary>
    public class WeatherAlert
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public bool IsActive => DateTime.UtcNow < ExpiresAt;
    }
}
