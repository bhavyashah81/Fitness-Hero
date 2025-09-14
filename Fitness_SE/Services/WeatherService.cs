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
    /// Service for handling weather-related operations
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly List<string> _summaries;
        private readonly Random _random;

        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
            _summaries = new List<string>
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", 
                "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
                "Sunny", "Cloudy", "Partly Cloudy", "Overcast",
                "Rainy", "Drizzling", "Stormy", "Windy", "Foggy", "Clear"
            };
            _random = new Random();
        }

        /// <summary>
        /// Gets weather forecast for specified number of days
        /// </summary>
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int days = 5)
        {
            _logger.LogInformation("Generating weather forecast for {Days} days", days);
            
            await Task.Delay(50); // Simulate async operation
            
            var forecasts = new List<WeatherForecast>();
            
            for (int i = 1; i <= days; i++)
            {
                var date = DateOnly.FromDateTime(DateTime.Now.AddDays(i));
                var temperature = _random.Next(-20, 55);
                var summary = _summaries[_random.Next(_summaries.Count)];
                
                var forecast = new WeatherForecast(date, temperature, summary);
                forecasts.Add(forecast);
                
                _logger.LogDebug("Generated forecast for {Date}: {Temperature}°C, {Summary}", 
                    date, temperature, summary);
            }
            
            return forecasts;
        }

        /// <summary>
        /// Gets extended weather information for a specific date
        /// </summary>
        public async Task<ExtendedWeatherInfo> GetExtendedWeatherInfoAsync(DateTime date)
        {
            _logger.LogInformation("Getting extended weather info for {Date}", date);
            
            await Task.Delay(30);
            
            var extendedInfo = new ExtendedWeatherInfo
            {
                Date = DateOnly.FromDateTime(date),
                TemperatureC = _random.Next(-10, 40),
                Summary = _summaries[_random.Next(_summaries.Count)]
            };
            
            return extendedInfo;
        }

        /// <summary>
        /// Gets all active weather alerts
        /// </summary>
        public async Task<IEnumerable<WeatherAlert>> GetActiveWeatherAlertsAsync()
        {
            _logger.LogInformation("Retrieving active weather alerts");
            
            await Task.Delay(20);
            
            var alerts = new List<WeatherAlert>();
            
            // Generate random alerts for demonstration
            if (_random.NextDouble() < 0.3) // 30% chance of alerts
            {
                var alert = new WeatherAlert
                {
                    Type = (WeatherAlertType)_random.Next(0, 6),
                    Severity = (AlertSeverity)_random.Next(0, 5),
                    Title = "Weather Advisory",
                    Description = "Sample weather alert for demonstration purposes",
                    AffectedAreas = new List<string> { "Local Area", "Surrounding Region" }
                };
                alerts.Add(alert);
            }
            
            return alerts.Where(a => a.IsActive);
        }

        /// <summary>
        /// Gets current weather conditions
        /// </summary>
        public async Task<WeatherForecast> GetCurrentWeatherAsync()
        {
            _logger.LogInformation("Getting current weather conditions");
            
            await Task.Delay(25);
            
            var currentWeather = new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now),
                _random.Next(-5, 35),
                _summaries[_random.Next(_summaries.Count)]
            );
            
            return currentWeather;
        }

        /// <summary>
        /// Determines if weather is suitable for outdoor workouts
        /// </summary>
        public async Task<bool> IsGoodWeatherForWorkoutAsync(DateTime date)
        {
            var weather = await GetExtendedWeatherInfoAsync(date);
            var result = weather.IsIdealForFitness();
            
            _logger.LogInformation("Weather suitability for workout on {Date}: {Result}", date, result);
            
            return result;
        }

        /// <summary>
        /// Gets workout recommendation based on weather
        /// </summary>
        public async Task<string> GetWorkoutRecommendationAsync(DateTime date)
        {
            var weather = await GetExtendedWeatherInfoAsync(date);
            var recommendation = weather.GetRecommendedWorkoutType();
            
            _logger.LogInformation("Workout recommendation for {Date}: {Recommendation}", date, recommendation);
            
            return recommendation;
        }

        /// <summary>
        /// Gets weather statistics for a date range
        /// </summary>
        public async Task<WeatherStatistics> GetWeatherStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Calculating weather statistics from {StartDate} to {EndDate}", startDate, endDate);
            
            await Task.Delay(100);
            
            var days = (endDate - startDate).Days + 1;
            var temperatures = new List<int>();
            
            for (int i = 0; i < days; i++)
            {
                temperatures.Add(_random.Next(-20, 55));
            }
            
            var statistics = new WeatherStatistics
            {
                StartDate = startDate,
                EndDate = endDate,
                AverageTemperature = temperatures.Average(),
                MinTemperature = temperatures.Min(),
                MaxTemperature = temperatures.Max(),
                TotalDays = days,
                GoodWorkoutDays = temperatures.Count(t => t >= 5 && t <= 35),
                RainyDays = _random.Next(0, days / 3),
                SunnyDays = _random.Next(days / 2, days)
            };
            
            return statistics;
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
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public int TotalDays { get; set; }
        public int GoodWorkoutDays { get; set; }
        public int RainyDays { get; set; }
        public int SunnyDays { get; set; }
        
        public double WorkoutSuitabilityPercentage => 
            TotalDays > 0 ? (double)GoodWorkoutDays / TotalDays * 100 : 0;
        
        public string GetWeatherSummary()
        {
            return $"Over {TotalDays} days: Avg temp {AverageTemperature:F1}°C, " +
                   $"{GoodWorkoutDays} good workout days ({WorkoutSuitabilityPercentage:F1}%)";
        }
    }
}
