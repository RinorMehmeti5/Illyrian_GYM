using AutoMapper;
using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentController(IPaymentRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
    {
        var payments = await _repo.GetAllWithDetailsAsync();
        return Ok(payments.Select(p => _mapper.Map<PaymentDto>(p)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDto>> GetPayment(int id)
    {
        var payment = await _repo.GetWithDetailsAsync(id);
        if (payment == null) return NotFound();
        return Ok(_mapper.Map<PaymentDto>(payment));
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> PostPayment(PaymentDto dto)
    {
        var payment = new Payment
        {
            UserId = dto.UserId,
            MembershipId = dto.MembershipId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            PaymentMethod = dto.PaymentMethod,
            TransactionId = dto.TransactionId,
            Notes = dto.Notes
        };

        await _repo.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, _mapper.Map<PaymentDto>(payment));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPayment(int id, PaymentDto dto)
    {
        if (id != dto.PaymentId) return BadRequest();

        var payment = await _repo.GetByIdAsync(id);
        if (payment == null) return NotFound();

        payment.UserId = dto.UserId;
        payment.MembershipId = dto.MembershipId;
        payment.Amount = dto.Amount;
        payment.PaymentDate = dto.PaymentDate;
        payment.PaymentMethod = dto.PaymentMethod;
        payment.TransactionId = dto.TransactionId;
        payment.Notes = dto.Notes;

        _repo.Update(payment);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _repo.GetByIdAsync(id);
        if (payment == null) return NotFound();

        _repo.Delete(payment);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
