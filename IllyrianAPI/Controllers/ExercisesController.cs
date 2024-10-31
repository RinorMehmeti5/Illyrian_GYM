// Controllers/ExercisesController.cs
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
        public ExercisesController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager
        ) : base(db, userManager)
        {
        }

        // GET: api/Exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDTO>>> GetExercises()
        {
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

        // GET: api/Exercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDTO>> GetExercise(int id)
        {
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

        // POST: api/Exercises
        [HttpPost]
        public async Task<ActionResult<ExerciseDTO>> PostExercise(ExerciseDTO exerciseDTO)
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

            // Map the saved entity back to DTO
            var createdExerciseDTO = new ExerciseDTO
            {
                ExerciseId = exercise.ExerciseId,
                ExerciseName = exercise.ExerciseName,
                Description = exercise.Description,
                MuscleGroup = exercise.MuscleGroup,
                DifficultyLevel = exercise.DifficultyLevel
            };

            return CreatedAtAction(nameof(GetExercise), new { id = exercise.ExerciseId }, createdExerciseDTO);
        }

        // PUT: api/Exercises/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, ExerciseDTO exerciseDTO)
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

        // DELETE: api/Exercises/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
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

        private bool ExerciseExists(int id)
        {
            return _db.Exercises.Any(e => e.ExerciseId == id);
        }
    }
}
