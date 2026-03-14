using AutoMapper;
using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.Exercise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ExercisesController> _logger;

    public ExercisesController(IExerciseRepository repo, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ExercisesController> logger)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetExercises()
    {
        try
        {
            var exercises = await _repo.GetAllAsync();
            return Ok(exercises.Select(e => _mapper.Map<ExerciseDto>(e)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting exercises");
            return StatusCode(500, new { message = "An error occurred while fetching exercises", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExerciseDto>> GetExercise(int id)
    {
        try
        {
            var exercise = await _repo.GetByIdAsync(id);
            if (exercise == null) return NotFound();
            return Ok(_mapper.Map<ExerciseDto>(exercise));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting exercise with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while fetching exercise with ID: {id}", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ExerciseDto>> PostExercise(ExerciseDto dto)
    {
        try
        {
            var exercise = new Exercise
            {
                ExerciseName = dto.ExerciseName,
                Description = dto.Description,
                MuscleGroup = dto.MuscleGroup,
                DifficultyLevel = dto.DifficultyLevel
            };

            await _repo.AddAsync(exercise);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExercise), new { id = exercise.ExerciseId }, _mapper.Map<ExerciseDto>(exercise));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating exercise");
            return StatusCode(500, new { message = "An error occurred while creating exercise", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutExercise(int id, ExerciseDto dto)
    {
        try
        {
            if (id != dto.ExerciseId) return BadRequest();

            var exercise = await _repo.GetByIdAsync(id);
            if (exercise == null) return NotFound();

            exercise.ExerciseName = dto.ExerciseName;
            exercise.Description = dto.Description;
            exercise.MuscleGroup = dto.MuscleGroup;
            exercise.DifficultyLevel = dto.DifficultyLevel;

            _repo.Update(exercise);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating exercise with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while updating exercise with ID: {id}", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        try
        {
            var exercise = await _repo.GetByIdAsync(id);
            if (exercise == null) return NotFound();

            _repo.Delete(exercise);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting exercise with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while deleting exercise with ID: {id}", error = ex.Message });
        }
    }
}
