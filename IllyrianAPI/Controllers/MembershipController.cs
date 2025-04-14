using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Models.Membership;
using System.Globalization;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembershipController : BaseController
    {
        private readonly ILogger<MembershipController> _logger;

        public MembershipController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager,
            ILogger<MembershipController> logger
        ) : base(db, userManager)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipDTO>>> GetMemberships()
        {
            try
            {
                _logger.LogInformation("Fetching all memberships");

                // First, get the raw data without any formatting
                var membershipsData = await _db.Memberships
                    .Include(m => m.User)
                    .Include(m => m.MembershipType)
                    .ToListAsync();

                // Then map and format the data after it's been retrieved from the database
                var memberships = membershipsData.Select(m => new MembershipDTO
                {
                    MembershipId = m.MembershipId,
                    UserId = m.UserId,
                    UserFullName = $"{m.User?.Firstname ?? ""} {m.User?.Lastname ?? ""}",
                    MembershipTypeId = m.MembershipTypeId,
                    MembershipTypeName = m.MembershipType?.Name,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    IsActive = m.IsActive ?? false,
                    Price = m.MembershipType?.Price ?? 0,
                    FormattedPrice = FormatPrice(m.MembershipType?.Price ?? 0),
                    FormattedStartDate = FormatDate(m.StartDate),
                    FormattedEndDate = FormatDate(m.EndDate),
                    DurationInDays = m.MembershipType?.DurationInDays ?? 0,
                    FormattedDuration = FormatDuration(m.MembershipType?.DurationInDays ?? 0)
                }).ToList();

                return memberships;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting memberships");
                return StatusCode(500, new { message = "An error occurred while fetching memberships", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipDTO>> GetMembership(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching membership with ID: {id}");

                // Get the membership with its related data
                var membershipData = await _db.Memberships
                    .Include(m => m.User)
                    .Include(m => m.MembershipType)
                    .FirstOrDefaultAsync(m => m.MembershipId == id);

                if (membershipData == null)
                {
                    return NotFound();
                }

                // Map and format the data
                var membership = new MembershipDTO
                {
                    MembershipId = membershipData.MembershipId,
                    UserId = membershipData.UserId,
                    UserFullName = $"{membershipData.User?.Firstname ?? ""} {membershipData.User?.Lastname ?? ""}",
                    MembershipTypeId = membershipData.MembershipTypeId,
                    MembershipTypeName = membershipData.MembershipType?.Name,
                    StartDate = membershipData.StartDate,
                    EndDate = membershipData.EndDate,
                    IsActive = membershipData.IsActive ?? false,
                    Price = membershipData.MembershipType?.Price ?? 0,
                    FormattedPrice = FormatPrice(membershipData.MembershipType?.Price ?? 0),
                    FormattedStartDate = FormatDate(membershipData.StartDate),
                    FormattedEndDate = FormatDate(membershipData.EndDate),
                    DurationInDays = membershipData.MembershipType?.DurationInDays ?? 0,
                    FormattedDuration = FormatDuration(membershipData.MembershipType?.DurationInDays ?? 0)
                };

                return membership;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting membership with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while fetching membership with ID: {id}", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MembershipDTO>> PostMembership(MembershipDTO membershipDTO)
        {
            try
            {
                var membership = new Memberships
                {
                    UserId = membershipDTO.UserId,
                    MembershipTypeId = membershipDTO.MembershipTypeId,
                    StartDate = membershipDTO.StartDate,
                    EndDate = membershipDTO.EndDate,
                    IsActive = membershipDTO.IsActive
                };

                _db.Memberships.Add(membership);
                await _db.SaveChangesAsync();

                // Reload the membership with related data
                //await _db.Entry(membership)
                //    .Reference(m => m.User)
                //    .LoadAsync();
                //await _db.Entry(membership)
                //    .Reference(m => m.MembershipType)
                //    .LoadAsync();

                var createdMembershipDTO = new MembershipDTO
                {
                    MembershipId = membership.MembershipId,
                    UserId = membership.UserId,
                    UserFullName = $"{membership.User?.Firstname ?? ""} {membership.User?.Lastname ?? ""}",
                    MembershipTypeId = membership.MembershipTypeId,
                    MembershipTypeName = membership.MembershipType?.Name,
                    StartDate = membership.StartDate,
                    EndDate = membership.EndDate,
                    IsActive = membership.IsActive ?? false,
                    Price = membership.MembershipType?.Price ?? 0,
                    FormattedPrice = FormatPrice(membership.MembershipType?.Price ?? 0),
                    FormattedStartDate = FormatDate(membership.StartDate),
                    FormattedEndDate = FormatDate(membership.EndDate),
                    DurationInDays = membership.MembershipType?.DurationInDays ?? 0,
                    FormattedDuration = FormatDuration(membership.MembershipType?.DurationInDays ?? 0)
                };

                return CreatedAtAction(nameof(GetMembership), new { id = membership.MembershipId }, createdMembershipDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating membership");
                return StatusCode(500, new { message = "An error occurred while creating membership", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembership(int id, MembershipDTO membershipDTO)
        {
            try
            {
                if (id != membershipDTO.MembershipId)
                {
                    return BadRequest();
                }

                var membership = await _db.Memberships.FindAsync(id);
                if (membership == null)
                {
                    return NotFound();
                }

                membership.UserId = membershipDTO.UserId;
                membership.MembershipTypeId = membershipDTO.MembershipTypeId;
                membership.StartDate = membershipDTO.StartDate;
                membership.EndDate = membershipDTO.EndDate;
                membership.IsActive = membershipDTO.IsActive;

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(id))
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
                _logger.LogError(ex, $"Error updating membership with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while updating membership with ID: {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            try
            {
                var membership = await _db.Memberships.FindAsync(id);
                if (membership == null)
                {
                    return NotFound();
                }

                _db.Memberships.Remove(membership);
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting membership with ID: {id}");
                return StatusCode(500, new { message = $"An error occurred while deleting membership with ID: {id}", error = ex.Message });
            }
        }

        private bool MembershipExists(int id)
        {
            return _db.Memberships.Any(e => e.MembershipId == id);
        }
    }
}