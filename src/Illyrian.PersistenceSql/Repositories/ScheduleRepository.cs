using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(IllyrianDbContext context) : base(context) { }
}
