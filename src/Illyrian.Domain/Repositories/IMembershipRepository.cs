using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Repositories;

public interface IMembershipRepository : IGenericRepository<Membership>
{
    Task<Membership?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Membership>> GetAllWithDetailsAsync();
}
