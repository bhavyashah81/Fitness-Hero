using Microsoft.EntityFrameworkCore;
using FitnessAppAPI.Models;

namespace FitnessAppAPI.Data
{
    public class FitnessDbContext : DbContext
    {
        public DbSet<Workout> Workouts { get; set; }

        public FitnessDbContext(DbContextOptions<FitnessDbContext> options)
            : base(options)
        {
        }
    }
}
