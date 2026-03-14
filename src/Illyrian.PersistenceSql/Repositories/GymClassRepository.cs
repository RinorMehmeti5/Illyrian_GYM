using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class GymClassRepository : GenericRepository<GymClass>, IGymClassRepository
{
    public GymClassRepository(IllyrianDbContext context) : base(context) { }
}
