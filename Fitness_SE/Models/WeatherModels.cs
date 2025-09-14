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
    /// Extended weather information with additional metrics
    /// </summary>
    public class ExtendedWeatherInfo : WeatherForecast
    {
        [JsonPropertyName("uvIndex")]
        public int UvIndex { get; set; }

        [JsonPropertyName("visibility")]
        public double Visibility { get; set; }

        [JsonPropertyName("cloudCover")]
        public int CloudCover { get; set; }

        [JsonPropertyName("dewPoint")]
        public double DewPoint { get; set; }

        [JsonPropertyName("feelsLike")]
        public int FeelsLike { get; set; }

        public ExtendedWeatherInfo() : base()
        {
            UvIndex = Random.Shared.Next(1, 12);
            Visibility = Math.Round(Random.Shared.NextDouble() * 10 + 5, 1);
            CloudCover = Random.Shared.Next(0, 101);
            DewPoint = Math.Round(TemperatureC - Random.Shared.NextDouble() * 10, 1);
            FeelsLike = TemperatureC + Random.Shared.Next(-5, 6);
        }

        /// <summary>
        /// Determines if conditions are ideal for fitness activities
        /// </summary>
        public bool IsIdealForFitness()
        {
            return IsGoodForOutdoorWorkout() && 
                   UvIndex < 8 && 
                   Visibility > 5 && 
                   CloudCover < 80;
        }

        /// <summary>
        /// Gets recommended workout type based on weather
        /// </summary>
        public string GetRecommendedWorkoutType()
        {
            if (!IsGoodForOutdoorWorkout()) return "Indoor workout recommended";
            if (TemperatureC < 10) return "Light jogging or brisk walking";
            if (TemperatureC > 25) return "Swimming or water activities";
            if (UvIndex > 7) return "Early morning or evening outdoor activities";
            return "Perfect for any outdoor activity";
        }
    }

    /// <summary>
    /// Weather alert information
    /// </summary>
    public class WeatherAlert
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("type")]
        public WeatherAlertType Type { get; set; }

        [JsonPropertyName("severity")]
        public AlertSeverity Severity { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("issuedAt")]
        public DateTime IssuedAt { get; set; }

        [JsonPropertyName("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        [JsonPropertyName("affectedAreas")]
        public List<string> AffectedAreas { get; set; } = new();

        public WeatherAlert()
        {
            Id = Guid.NewGuid();
            IssuedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddHours(6);
        }

        public bool IsActive => DateTime.UtcNow < ExpiresAt;

        public TimeSpan TimeUntilExpiry => ExpiresAt - DateTime.UtcNow;
    }

    /// <summary>
    /// Weather alert types
    /// </summary>
    public enum WeatherAlertType
    {
        Temperature,
        Wind,
        Precipitation,
        Visibility,
        AirQuality,
        UvIndex
    }

    /// <summary>
    /// Alert severity levels
    /// </summary>
    public enum AlertSeverity
    {
        Info,
        Minor,
        Moderate,
        Severe,
        Extreme
    }
}
