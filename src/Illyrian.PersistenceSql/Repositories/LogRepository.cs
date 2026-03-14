using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;

namespace Illyrian.PersistenceSql.Repositories;

public class LogRepository : ILogRepository
{
    private readonly IllyrianDbContext _context;

    public LogRepository(IllyrianDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LogEntry log)
    {
        await _context.Logs.AddAsync(log);
    }

    public void Update(LogEntry log)
    {
        _context.Logs.Update(log);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
