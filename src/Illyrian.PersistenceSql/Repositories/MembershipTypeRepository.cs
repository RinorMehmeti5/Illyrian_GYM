using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class MembershipTypeRepository : GenericRepository<MembershipType>, IMembershipTypeRepository
{
    public MembershipTypeRepository(IllyrianDbContext context) : base(context) { }
}
