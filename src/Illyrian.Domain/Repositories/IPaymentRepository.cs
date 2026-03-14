using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<Payment?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Payment>> GetAllWithDetailsAsync();
}
