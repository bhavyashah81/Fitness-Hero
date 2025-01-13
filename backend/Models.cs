namespace FitnessAppAPI.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; } // In minutes
        public int CaloriesBurned { get; set; }
    }
}
