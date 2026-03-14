using AutoMapper;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.Schedule;
using Illyrian.RestApi.Utils.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(IScheduleRepository repo, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ScheduleController> logger)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedules()
    {
        try
        {
            var schedules = await _repo.GetAllAsync();
            return Ok(schedules.Select(s => _mapper.Map<ScheduleDto>(s)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedules");
            return StatusCode(500, new { message = "An error occurred while fetching schedules", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleDto>> GetSchedule(int id)
    {
        try
        {
            var schedule = await _repo.GetByIdAsync(id);
            if (schedule == null) return NotFound();
            return Ok(_mapper.Map<ScheduleDto>(schedule));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while fetching schedule with ID: {id}", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleDto>> PostSchedule([FromBody] CreateScheduleRequest request)
    {
        try
        {
            if (!FormattingHelpers.TryParseTimeString(request.StartTime, out DateTime startTime) ||
                !FormattingHelpers.TryParseTimeString(request.EndTime, out DateTime endTime))
            {
                return BadRequest(new { message = "Invalid time format. Use HH:MM format." });
            }

            var schedule = new Domain.Entities.Schedule
            {
                StartTime = startTime,
                EndTime = endTime,
                DayOfWeek = request.DayOfWeek
            };

            await _repo.AddAsync(schedule);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.ScheduleId }, _mapper.Map<ScheduleDto>(schedule));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating schedule");
            return StatusCode(500, new { message = "An error occurred while creating schedule", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSchedule(int id, [FromBody] UpdateScheduleRequest request)
    {
        try
        {
            if (id != request.ScheduleId) return BadRequest();

            var schedule = await _repo.GetByIdAsync(id);
            if (schedule == null) return NotFound();

            if (!FormattingHelpers.TryParseTimeString(request.StartTime, out DateTime startTime) ||
                !FormattingHelpers.TryParseTimeString(request.EndTime, out DateTime endTime))
            {
                return BadRequest(new { message = "Invalid time format. Use HH:MM format." });
            }

            schedule.StartTime = startTime;
            schedule.EndTime = endTime;
            schedule.DayOfWeek = request.DayOfWeek;

            _repo.Update(schedule);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating schedule with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while updating schedule with ID: {id}", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        try
        {
            var schedule = await _repo.GetByIdAsync(id);
            if (schedule == null) return NotFound();

            _repo.Delete(schedule);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting schedule with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while deleting schedule with ID: {id}", error = ex.Message });
        }
    }
}
