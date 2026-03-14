using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class UserClassRepository : GenericRepository<UserClass>, IUserClassRepository
{
    public UserClassRepository(IllyrianDbContext context) : base(context) { }
}
