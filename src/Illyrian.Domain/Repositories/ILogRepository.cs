using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Repositories;

public interface ILogRepository
{
    Task AddAsync(LogEntry log);
    void Update(LogEntry log);
    Task<int> SaveChangesAsync();
}
