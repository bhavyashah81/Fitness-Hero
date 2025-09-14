using FitnessAppAPI.Models;
using System.Text.Json;

namespace FitnessAppAPI.Services
{
    /// <summary>
    /// Interface for weather-related operations
    /// </summary>
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int days = 5);
        Task<ExtendedWeatherInfo> GetExtendedWeatherInfoAsync(DateTime date);
        Task<IEnumerable<WeatherAlert>> GetActiveWeatherAlertsAsync();
        Task<WeatherForecast> GetCurrentWeatherAsync();
        Task<bool> IsGoodWeatherForWorkoutAsync(DateTime date);
        Task<string> GetWorkoutRecommendationAsync(DateTime date);
        Task<WeatherStatistics> GetWeatherStatisticsAsync(DateTime startDate, DateTime endDate);
    }

    /// <summary>
    /// Simple weather service implementation
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly Random _random = new();

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int days = 5)
        {
            await Task.Delay(10);
            var forecasts = new List<WeatherForecast>();
            
            for (int i = 1; i <= days; i++)
            {
                forecasts.Add(new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    _random.Next(-20, 55),
                    "Sample"
                ));
            }
            
            return forecasts;
        }

        public async Task<ExtendedWeatherInfo> GetExtendedWeatherInfoAsync(DateTime date)
        {
            await Task.Delay(10);
            return new ExtendedWeatherInfo();
        }

        public async Task<IEnumerable<WeatherAlert>> GetActiveWeatherAlertsAsync()
        {
            await Task.Delay(10);
            return new List<WeatherAlert>();
        }

        public async Task<WeatherForecast> GetCurrentWeatherAsync()
        {
            await Task.Delay(10);
            return new WeatherForecast();
        }

        public async Task<bool> IsGoodWeatherForWorkoutAsync(DateTime date)
        {
            await Task.Delay(10);
            return true;
        }

        public async Task<string> GetWorkoutRecommendationAsync(DateTime date)
        {
            await Task.Delay(10);
            return "Good weather for workout";
        }

        public async Task<WeatherStatistics> GetWeatherStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            await Task.Delay(10);
            return new WeatherStatistics();
        }
    }

    /// <summary>
    /// Weather statistics model
    /// </summary>
    public class WeatherStatistics
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double AverageTemperature { get; set; }
        public int TotalDays { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public int GoodWorkoutDays { get; set; }
        public int RainyDays { get; set; }
        public int SunnyDays { get; set; }
        
        public double WorkoutSuitabilityPercentage => 
            TotalDays > 0 ? (double)GoodWorkoutDays / TotalDays * 100 : 0;
        
        public string GetWeatherSummary()
        {
            return $"Over {TotalDays} days: Avg temp {AverageTemperature:F1}°C";
        }
        
        public bool IsGoodWeatherMonth()
        {
            return WorkoutSuitabilityPercentage > 60;
        }
        
        public string GetTemperatureCategory()
        {
            if (AverageTemperature < 5) return "Cold";
            if (AverageTemperature < 15) return "Cool";
            if (AverageTemperature < 25) return "Warm";
            return "Hot";
        }
    }
}
