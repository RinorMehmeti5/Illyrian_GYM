using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Repositories;

public interface ITestItemRepository
{
    Task<IEnumerable<TestItem>> GetAllAsync();
    Task<TestItem?> GetByIdAsync(int id);
}
