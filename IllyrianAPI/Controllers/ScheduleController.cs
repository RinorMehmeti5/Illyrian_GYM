// Controllers/ScheduleController.cs
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
        public ScheduleController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager
        ) : base(db, userManager)
        {
        }

        // GET: api/Schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            var schedules = await _db.Schedule
                .Select(s => new ScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    DayOfWeek = s.DayOfWeek,
                    FormattedStartTime = FormatDate(s.StartTime),
                    FormattedEndTime = FormatDate(s.EndTime),
                    Duration = FormatDuration(s.EndTime - s.StartTime)
                })
                .ToListAsync();

            return schedules;
        }

        // GET: api/Schedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            var schedule = await _db.Schedule
                .Where(s => s.ScheduleId == id)
                .Select(s => new ScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    DayOfWeek = s.DayOfWeek,
                    FormattedStartTime = FormatDate(s.StartTime),
                    FormattedEndTime = FormatDate(s.EndTime),
                    Duration = FormatDuration(s.EndTime - s.StartTime)
                })
                .FirstOrDefaultAsync();

            if (schedule == null)
            {
                return NotFound();
            }

            return schedule;
        }

        // POST: api/Schedule
        [HttpPost]
        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO scheduleDTO)
        {
            var schedule = new Schedule
            {
                StartTime = scheduleDTO.StartTime,
                EndTime = scheduleDTO.EndTime,
                DayOfWeek = scheduleDTO.DayOfWeek
            };

            _db.Schedule.Add(schedule);
            await _db.SaveChangesAsync();

            // Map the saved entity back to DTO
            var createdScheduleDTO = new ScheduleDTO
            {
                ScheduleId = schedule.ScheduleId,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                DayOfWeek = schedule.DayOfWeek,
                FormattedStartTime = FormatDate(schedule.StartTime),
                FormattedEndTime = FormatDate(schedule.EndTime),
                Duration = FormatDuration(schedule.EndTime - schedule.StartTime)
            };

            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.ScheduleId }, createdScheduleDTO);
        }

        // PUT: api/Schedule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO scheduleDTO)
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

        // DELETE: api/Schedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
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

        private bool ScheduleExists(int id)
        {
            return _db.Schedule.Any(e => e.ScheduleId == id);
        }

        private string FormatDuration(TimeSpan duration)
        {
            int hours = duration.Hours;
            int minutes = duration.Minutes;
            return $"{hours}h {minutes}m";
        }
    }
}
