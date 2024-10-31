using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Models.MembershipType;
using System.Globalization;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembershipTypesController : BaseController
    {
        public MembershipTypesController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager
        ) : base(db, userManager)
        {
        }

        // GET: api/MembershipTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembershipTypeDTO>>> GetMembershipTypes()
        {
            var membershipTypes = await _db.MembershipTypes
                .Select(mt => new MembershipTypeDTO
                {
                    MembershipTypeID = mt.MembershipTypeId,
                    Name = mt.Name,
                    Description = mt.Description,
                    FormattedDuration = FormatDuration(mt.DurationInDays),
                    FormattedPrice = FormatPrice(mt.Price)
                })
                .ToListAsync();

            return membershipTypes;
        }

        // GET: api/MembershipTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MembershipTypeDTO>> GetMembershipType(int id)
        {
            var membershipType = await _db.MembershipTypes
                .Where(mt => mt.MembershipTypeId == id)
                .Select(mt => new MembershipTypeDTO
                {
                    MembershipTypeID = mt.MembershipTypeId,
                    Name = mt.Name,
                    Description = mt.Description,
                    FormattedDuration = FormatDuration(mt.DurationInDays),
                    FormattedPrice = FormatPrice(mt.Price)
                })
                .FirstOrDefaultAsync();

            if (membershipType == null)
            {
                return NotFound();
            }

            return membershipType;
        }

        // POST: api/MembershipTypes
        [HttpPost]
        public async Task<ActionResult<MembershipTypeDTO>> PostMembershipType(MembershipTypeDTO membershipTypeDTO)
        {
            var membershipType = new MembershipTypes
            {
                Name = membershipTypeDTO.Name,
                Description = membershipTypeDTO.Description,
                DurationInDays = membershipTypeDTO.DurationInDays,
                Price = membershipTypeDTO.Price
            };

            _db.MembershipTypes.Add(membershipType);
            await _db.SaveChangesAsync();

            membershipTypeDTO.MembershipTypeID = membershipType.MembershipTypeId;
            membershipTypeDTO.FormattedDuration = FormatDuration(membershipType.DurationInDays);
            membershipTypeDTO.FormattedPrice = FormatPrice(membershipType.Price);

            return CreatedAtAction(nameof(GetMembershipType), new { id = membershipType.MembershipTypeId }, membershipTypeDTO);
        }

        // PUT: api/MembershipTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembershipType(int id, MembershipTypeDTO membershipTypeDTO)
        {
            if (id != membershipTypeDTO.MembershipTypeID)
            {
                return BadRequest();
            }

            var membershipType = await _db.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            membershipType.Name = membershipTypeDTO.Name;
            membershipType.Description = membershipTypeDTO.Description;
            membershipType.DurationInDays = membershipTypeDTO.DurationInDays;
            membershipType.Price = membershipTypeDTO.Price;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipTypeExists(id))
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

        // DELETE: api/MembershipTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembershipType(int id)
        {
            var membershipType = await _db.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            _db.MembershipTypes.Remove(membershipType);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool MembershipTypeExists(int id)
        {
            return _db.MembershipTypes.Any(e => e.MembershipTypeId == id);
        }
    }
}