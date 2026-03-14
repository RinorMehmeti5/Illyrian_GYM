using Illyrian.Persistence.General;
using Illyrian.PersistenceSql.Context;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.RestApi.Tests.Helpers;

public static class TestDbContextFactory
{
    public static IllyrianDbContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<IllyrianDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new IllyrianDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
