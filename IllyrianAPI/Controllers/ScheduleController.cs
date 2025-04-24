using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Models.Schedule;
using System.Globalization;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : BaseController
    {
        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager,
            ILogger<ScheduleController> logger
        ) : base(db, userManager)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            try
            {
                _logger.LogInformation("Fetching all schedules");

                var schedulesData = await _db.Schedule.ToListAsync();

                var schedules = schedulesData.Select(s => new ScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    DayOfWeek = s.DayOfWeek,
                    FormattedStartTime = FormatTime(s.StartTime),
                    FormattedEndTime = FormatTime(s.EndTime),
                    FormattedDayOfWeek = s.DayOfWeek
                }).ToList();

                return schedules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting schedules");
                return StatusCode(500, new { message = "An error occurred while fetching schedules", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching schedule with ID: {id}");

                var scheduleData = await _db.Schedule.FirstOrDefaultAsync(s => s.ScheduleId == id);

                if (scheduleData == null)
                {
                    return NotFound();
                }

                var schedule = new ScheduleDTO
                {
                    ScheduleId = scheduleData.ScheduleId,
                    StartTime = scheduleData.StartTime,
                    EndTime = scheduleData.EndTime,
                    DayOfWeek = scheduleData.DayOfWeek,
                    FormattedStartTime = FormatTime(scheduleData.StartTime),
                    FormattedEndTime = FormatTime(scheduleData.EndTime),
                    FormattedDayOfWeek = scheduleData.DayOfWeek
                };

                return schedule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting schedule with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while fetching schedule with ID: {id}", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO scheduleDTO)
        {
            try
            {
                var schedule = new Schedule
                {
                    StartTime = scheduleDTO.StartTime,
                    EndTime = scheduleDTO.EndTime,
                    DayOfWeek = scheduleDTO.DayOfWeek
                };

                _db.Schedule.Add(schedule);
                await _db.SaveChangesAsync();

                var createdScheduleDTO = new ScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    DayOfWeek = schedule.DayOfWeek,
                    FormattedStartTime = FormatTime(schedule.StartTime),
                    FormattedEndTime = FormatTime(schedule.EndTime),
                    FormattedDayOfWeek = schedule.DayOfWeek
                };

                return CreatedAtAction(nameof(GetSchedule), new { id = schedule.ScheduleId }, createdScheduleDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating schedule");
                return StatusCode(500, new { message = "An error occurred while creating schedule", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO scheduleDTO)
        {
            try
            {
                if (id != scheduleDTO.ScheduleId)
                {
                    return BadRequest();
                }

                var schedule = await _db.Schedule.FindAsync(id);
                if (schedule == null)
                {
                    return NotFound();
                }

                schedule.StartTime = scheduleDTO.StartTime;
                schedule.EndTime = scheduleDTO.EndTime;
                schedule.DayOfWeek = scheduleDTO.DayOfWeek;

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(id))
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
                _logger.LogError(ex, $"Error updating schedule with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while updating schedule with ID: {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                var schedule = await _db.Schedule.FindAsync(id);
                if (schedule == null)
                {
                    return NotFound();
                }

                _db.Schedule.Remove(schedule);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting schedule with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while deleting schedule with ID: {id}", error = ex.Message });
            }
        }

        private bool ScheduleExists(int id)
        {
            return _db.Schedule.Any(e => e.ScheduleId == id);
        }

        [NonAction]
        private string FormatTime(DateTime time)
        {
            return time.ToString("hh:mm tt");
        }
    }
}