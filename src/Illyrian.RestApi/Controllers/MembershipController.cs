using AutoMapper;
using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.Membership;
using Illyrian.Persistence.MembershipType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MembershipController : ControllerBase
{
    private readonly IMembershipRepository _membershipRepo;
    private readonly IMembershipTypeRepository _membershipTypeRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<MembershipController> _logger;

    public MembershipController(
        IMembershipRepository membershipRepo,
        IMembershipTypeRepository membershipTypeRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<MembershipController> logger)
    {
        _membershipRepo = membershipRepo;
        _membershipTypeRepo = membershipTypeRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MembershipDto>>> GetMemberships()
    {
        try
        {
            var memberships = await _membershipRepo.GetAllWithDetailsAsync();
            return Ok(memberships.Select(m => _mapper.Map<MembershipDto>(m)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting memberships");
            return StatusCode(500, new { message = "An error occurred while fetching memberships", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MembershipDto>> GetMembership(int id)
    {
        try
        {
            var membership = await _membershipRepo.GetWithDetailsAsync(id);
            if (membership == null) return NotFound();
            return Ok(_mapper.Map<MembershipDto>(membership));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting membership with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while fetching membership with ID: {id}", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<MembershipDto>> PostMembership(MembershipDto membershipDto)
    {
        try
        {
            var membership = new Membership
            {
                UserId = membershipDto.UserId,
                MembershipTypeId = membershipDto.MembershipTypeId,
                StartDate = membershipDto.StartDate,
                EndDate = membershipDto.EndDate,
                IsActive = membershipDto.IsActive
            };

            await _membershipRepo.AddAsync(membership);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembership), new { id = membership.MembershipId }, _mapper.Map<MembershipDto>(membership));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating membership");
            return StatusCode(500, new { message = "An error occurred while creating membership", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMembership(int id, MembershipDto membershipDto)
    {
        try
        {
            if (id != membershipDto.MembershipId) return BadRequest();

            var membership = await _membershipRepo.GetByIdAsync(id);
            if (membership == null) return NotFound();

            membership.UserId = membershipDto.UserId;
            membership.MembershipTypeId = membershipDto.MembershipTypeId;
            membership.StartDate = membershipDto.StartDate;
            membership.EndDate = membershipDto.EndDate;
            membership.IsActive = membershipDto.IsActive;

            _membershipRepo.Update(membership);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating membership with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while updating membership with ID: {id}", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMembership(int id)
    {
        try
        {
            var membership = await _membershipRepo.GetByIdAsync(id);
            if (membership == null) return NotFound();

            _membershipRepo.Delete(membership);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting membership with ID: {Id}", id);
            return StatusCode(500, new { message = $"An error occurred while deleting membership with ID: {id}", error = ex.Message });
        }
    }

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<MembershipTypeDto>>> GetMembershipTypes()
    {
        try
        {
            var types = await _membershipTypeRepo.GetAllAsync();
            return Ok(types.Select(mt => _mapper.Map<MembershipTypeDto>(mt)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting membership types");
            return StatusCode(500, new { message = "An error occurred while fetching membership types", error = ex.Message });
        }
    }
}
