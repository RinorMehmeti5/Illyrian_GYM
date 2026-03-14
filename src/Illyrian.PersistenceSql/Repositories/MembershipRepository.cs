using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.PersistenceSql.Repositories;

public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
{
    public MembershipRepository(IllyrianDbContext context) : base(context) { }

    public async Task<Membership?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(m => m.MembershipType)
            .FirstOrDefaultAsync(m => m.MembershipId == id);
    }

    public async Task<IEnumerable<Membership>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(m => m.MembershipType)
            .ToListAsync();
    }
}
