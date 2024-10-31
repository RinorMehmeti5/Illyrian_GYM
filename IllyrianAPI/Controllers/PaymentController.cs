using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IllyrianAPI.Models.Payment;
using System.Globalization;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : BaseController
    {
        public PaymentController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager
        ) : base(db, userManager)
        {
        }

        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments()
        {
            var payments = await _db.Payments
                .Include(p => p.User)
                .Include(p => p.Membership)
                    .ThenInclude(m => m.MembershipType)
                .Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    UserId = p.UserId,
                    UserFullName = $"{p.User.Firstname} {p.User.Lastname}",
                    MembershipId = p.MembershipId,
                    MembershipTypeName = p.Membership.MembershipType.Name,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod,
                    TransactionId = p.TransactionId,
                    Notes = p.Notes,
                    FormattedAmount = FormatPrice(p.Amount),
                    FormattedPaymentDate = FormatDate(p.PaymentDate)
                })
                .ToListAsync();

            return payments;
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPayment(int id)
        {
            var payment = await _db.Payments
                .Include(p => p.User)
                .Include(p => p.Membership)
                    .ThenInclude(m => m.MembershipType)
                .Where(p => p.PaymentId == id)
                .Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    UserId = p.UserId,
                    UserFullName = $"{p.User.Firstname} {p.User.Lastname}",
                    MembershipId = p.MembershipId,
                    MembershipTypeName = p.Membership.MembershipType.Name,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    PaymentMethod = p.PaymentMethod,
                    TransactionId = p.TransactionId,
                    Notes = p.Notes,
                    FormattedAmount = FormatPrice(p.Amount),
                    FormattedPaymentDate = FormatDate(p.PaymentDate)
                })
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // POST: api/Payment
        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> PostPayment(PaymentDTO paymentDTO)
        {
            var payment = new Payments
            {
                UserId = paymentDTO.UserId,
                MembershipId = paymentDTO.MembershipId,
                Amount = paymentDTO.Amount,
                PaymentDate = paymentDTO.PaymentDate,
                PaymentMethod = paymentDTO.PaymentMethod,
                TransactionId = paymentDTO.TransactionId,
                Notes = paymentDTO.Notes
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            // Reload the payment with related data
            //await _db.Entry(payment)
            //    .Reference(p => p.User)
            //    .LoadAsync();
            //await _db.Entry(payment)
            //    .Reference(p => p.Membership)
            //    .Query()
            //    .Include(m => m.MembershipType)
            //    .LoadAsync();

            var createdPaymentDTO = new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                UserId = payment.UserId,
                UserFullName = $"{payment.User.Firstname} {payment.User.Lastname}",
                MembershipId = payment.MembershipId,
                MembershipTypeName = payment.Membership.MembershipType.Name,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                Notes = payment.Notes,
                FormattedAmount = FormatPrice(payment.Amount),
                FormattedPaymentDate = FormatDate(payment.PaymentDate)
            };

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, createdPaymentDTO);
        }

        // PUT: api/Payment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, PaymentDTO paymentDTO)
        {
            if (id != paymentDTO.PaymentId)
            {
                return BadRequest();
            }

            var payment = await _db.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.UserId = paymentDTO.UserId;
            payment.MembershipId = paymentDTO.MembershipId;
            payment.Amount = paymentDTO.Amount;
            payment.PaymentDate = paymentDTO.PaymentDate;
            payment.PaymentMethod = paymentDTO.PaymentMethod;
            payment.TransactionId = paymentDTO.TransactionId;
            payment.Notes = paymentDTO.Notes;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _db.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _db.Payments.Remove(payment);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _db.Payments.Any(e => e.PaymentId == id);
        }
    }
}