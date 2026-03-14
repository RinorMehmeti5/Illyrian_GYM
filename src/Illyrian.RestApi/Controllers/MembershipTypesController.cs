using AutoMapper;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.MembershipType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MembershipTypesController : ControllerBase
{
    private readonly IMembershipTypeRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MembershipTypesController(IMembershipTypeRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MembershipTypeDto>>> GetMembershipTypes()
    {
        var types = await _repo.GetAllAsync();
        return Ok(types.Select(mt => _mapper.Map<MembershipTypeDto>(mt)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MembershipTypeDto>> GetMembershipType(int id)
    {
        var mt = await _repo.GetByIdAsync(id);
        if (mt == null) return NotFound();
        return Ok(_mapper.Map<MembershipTypeDto>(mt));
    }

    [HttpPost]
    public async Task<ActionResult<MembershipTypeDto>> PostMembershipType(MembershipTypeDto dto)
    {
        var entity = new Domain.Entities.MembershipType
        {
            Name = dto.Name!,
            Description = dto.Description,
            DurationInDays = dto.DurationInDays,
            Price = dto.Price
        };

        await _repo.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMembershipType), new { id = entity.MembershipTypeId }, _mapper.Map<MembershipTypeDto>(entity));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMembershipType(int id, MembershipTypeDto dto)
    {
        if (id != dto.MembershipTypeID) return BadRequest();

        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        entity.Name = dto.Name!;
        entity.Description = dto.Description;
        entity.DurationInDays = dto.DurationInDays;
        entity.Price = dto.Price;

        _repo.Update(entity);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMembershipType(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        _repo.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
