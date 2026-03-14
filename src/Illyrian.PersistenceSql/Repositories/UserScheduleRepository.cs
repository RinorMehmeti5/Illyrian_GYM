using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class UserScheduleRepository : GenericRepository<UserSchedule>, IUserScheduleRepository
{
    public UserScheduleRepository(IllyrianDbContext context) : base(context) { }
}
