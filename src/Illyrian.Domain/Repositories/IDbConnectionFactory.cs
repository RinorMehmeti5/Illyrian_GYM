using System.Data;

namespace Illyrian.Domain.Repositories;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
