using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(IllyrianDbContext context) : base(context) { }
}
