using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Models.Exercise;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExercisesController : BaseController
    {
        private readonly ILogger<ExercisesController> _logger;

        public ExercisesController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager,
            ILogger<ExercisesController> logger
        ) : base(db, userManager)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDTO>>> GetExercises()
        {
            try
            {
                _logger.LogInformation("Fetching all exercises");

                var exercises = await _db.Exercises
                    .Select(e => new ExerciseDTO
                    {
                        ExerciseId = e.ExerciseId,
                        ExerciseName = e.ExerciseName,
                        Description = e.Description,
                        MuscleGroup = e.MuscleGroup,
                        DifficultyLevel = e.DifficultyLevel
                    })
                    .ToListAsync();

                return exercises;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exercises");
                return StatusCode(500, new { message = "An error occurred while fetching exercises", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDTO>> GetExercise(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching exercise with ID: {id}");

                var exercise = await _db.Exercises
                    .Where(e => e.ExerciseId == id)
                    .Select(e => new ExerciseDTO
                    {
                        ExerciseId = e.ExerciseId,
                        ExerciseName = e.ExerciseName,
                        Description = e.Description,
                        MuscleGroup = e.MuscleGroup,
                        DifficultyLevel = e.DifficultyLevel
                    })
                    .FirstOrDefaultAsync();

                if (exercise == null)
                {
                    return NotFound();
                }

                return exercise;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting exercise with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while fetching exercise with ID: {id}", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExerciseDTO>> PostExercise(ExerciseDTO exerciseDTO)
        {
            try
            {
                var exercise = new Exercises
                {
                    ExerciseName = exerciseDTO.ExerciseName,
                    Description = exerciseDTO.Description,
                    MuscleGroup = exerciseDTO.MuscleGroup,
                    DifficultyLevel = exerciseDTO.DifficultyLevel
                };

                _db.Exercises.Add(exercise);
                await _db.SaveChangesAsync();

                exerciseDTO.ExerciseId = exercise.ExerciseId;

                return CreatedAtAction(nameof(GetExercise), new { id = exercise.ExerciseId }, exerciseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exercise");
                return StatusCode(500, new { message = "An error occurred while creating exercise", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, ExerciseDTO exerciseDTO)
        {
            try
            {
                if (id != exerciseDTO.ExerciseId)
                {
                    return BadRequest();
                }

                var exercise = await _db.Exercises.FindAsync(id);
                if (exercise == null)
                {
                    return NotFound();
                }

                exercise.ExerciseName = exerciseDTO.ExerciseName;
                exercise.Description = exerciseDTO.Description;
                exercise.MuscleGroup = exerciseDTO.MuscleGroup;
                exercise.DifficultyLevel = exerciseDTO.DifficultyLevel;

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating exercise with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while updating exercise with ID: {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            try
            {
                var exercise = await _db.Exercises.FindAsync(id);
                if (exercise == null)
                {
                    return NotFound();
                }

                _db.Exercises.Remove(exercise);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting exercise with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while deleting exercise with ID: {id}", error = ex.Message });
            }
        }

        private bool ExerciseExists(int id)
        {
            return _db.Exercises.Any(e => e.ExerciseId == id);
        }
    }
}
