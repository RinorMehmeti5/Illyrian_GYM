using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.PersistenceSql.Repositories;

public class TestItemRepository : ITestItemRepository
{
    private readonly IllyrianDbContext _context;

    public TestItemRepository(IllyrianDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TestItem>> GetAllAsync()
    {
        return await _context.TestItems.ToListAsync();
    }

    public async Task<TestItem?> GetByIdAsync(int id)
    {
        return await _context.TestItems.FindAsync(id);
    }
}
