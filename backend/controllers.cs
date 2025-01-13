using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessAppAPI.Models;  // For Workout model
using FitnessAppAPI.Data;    // For FitnessDbContext

namespace FitnessAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly FitnessDbContext _context;

        public WorkoutController(FitnessDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkouts()
        {
            return Ok(await _context.Workouts.ToListAsync());
        }
    }
}
