using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.PersistenceSql.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(IllyrianDbContext context) : base(context) { }

    public async Task<Payment?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Membership)
                .ThenInclude(m => m!.MembershipType)
            .FirstOrDefaultAsync(p => p.PaymentId == id);
    }

    public async Task<IEnumerable<Payment>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(p => p.Membership)
                .ThenInclude(m => m!.MembershipType)
            .ToListAsync();
    }
}
