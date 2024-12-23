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
        public MembershipController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager
        ) : base(db, userManager)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipDTO>>> GetMemberships()
        {
            var memberships = await _db.Memberships
                .Include(m => m.User)
                .Include(m => m.MembershipType)
                .Select(m => new MembershipDTO
                {
                    MembershipId = m.MembershipId,
                    UserId = m.UserId,
                    UserFullName = $"{m.User.Firstname} {m.User.Lastname}",
                    MembershipTypeId = m.MembershipTypeId,
                    MembershipTypeName = m.MembershipType.Name,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    IsActive = m.IsActive ?? false,
                    Price = m.MembershipType.Price,
                    FormattedPrice = FormatPrice(m.MembershipType.Price),
                    FormattedStartDate = FormatDate(m.StartDate),
                    FormattedEndDate = FormatDate(m.EndDate),
                    DurationInDays = m.MembershipType.DurationInDays,
                    FormattedDuration = FormatDuration(m.MembershipType.DurationInDays)
                })
                .ToListAsync();

            return memberships;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipDTO>> GetMembership(int id)
        {
            var membership = await _db.Memberships
                .Include(m => m.User)
                .Include(m => m.MembershipType)
                .Where(m => m.MembershipId == id)
                .Select(m => new MembershipDTO
                {
                    MembershipId = m.MembershipId,
                    UserId = m.UserId,
                    UserFullName = $"{m.User.Firstname} {m.User.Lastname}",
                    MembershipTypeId = m.MembershipTypeId,
                    MembershipTypeName = m.MembershipType.Name,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    IsActive = m.IsActive ?? false,
                    Price = m.MembershipType.Price,
                    FormattedPrice = FormatPrice(m.MembershipType.Price),
                    FormattedStartDate = FormatDate(m.StartDate),
                    FormattedEndDate = FormatDate(m.EndDate),
                    DurationInDays = m.MembershipType.DurationInDays,
                    FormattedDuration = FormatDuration(m.MembershipType.DurationInDays)
                })
                .FirstOrDefaultAsync();

            if (membership == null)
            {
                return NotFound();
            }

            return membership;
        }

        [HttpPost]
        public async Task<ActionResult<MembershipDTO>> PostMembership(MembershipDTO membershipDTO)
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

            await _db.Entry(membership)
                .Reference(m => m.User)
                .LoadAsync();
            await _db.Entry(membership)
                .Reference(m => m.MembershipType)
                .LoadAsync();

            var createdMembershipDTO = new MembershipDTO
            {
                MembershipId = membership.MembershipId,
                UserId = membership.UserId,
                UserFullName = $"{membership.User.Firstname} {membership.User.Lastname}",
                MembershipTypeId = membership.MembershipTypeId,
                MembershipTypeName = membership.MembershipType.Name,
                StartDate = membership.StartDate,
                EndDate = membership.EndDate,
                IsActive = membership.IsActive ?? false,
                Price = membership.MembershipType.Price,
                FormattedPrice = FormatPrice(membership.MembershipType.Price),
                FormattedStartDate = FormatDate(membership.StartDate),
                FormattedEndDate = FormatDate(membership.EndDate),
                DurationInDays = membership.MembershipType.DurationInDays,
                FormattedDuration = FormatDuration(membership.MembershipType.DurationInDays)
            };

            return CreatedAtAction(nameof(GetMembership), new { id = membership.MembershipId }, createdMembershipDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembership(int id, MembershipDTO membershipDTO)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
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

        private bool MembershipExists(int id)
        {
            return _db.Memberships.Any(e => e.MembershipId == id);
        }
    }
}