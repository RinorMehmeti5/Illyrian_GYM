using Illyrian.Domain.Entities;
using Illyrian.PersistenceSql.Repositories;
using Illyrian.RestApi.Tests.Helpers;
using Xunit;

namespace Illyrian.RestApi.Tests.Repositories;

public class MembershipRepositoryTests
{
    [Fact]
    public async Task AddAsync_And_GetByIdAsync_ReturnsMembership()
    {
        using var context = TestDbContextFactory.Create();
        var repo = new MembershipRepository(context);

        var membershipType = new MembershipType
        {
            MembershipTypeId = 1,
            Name = "Basic",
            DurationInDays = 30,
            Price = 29.99m
        };
        context.MembershipTypes.Add(membershipType);
        await context.SaveChangesAsync();

        var membership = new Membership
        {
            UserId = "user-1",
            MembershipTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            IsActive = true
        };

        await repo.AddAsync(membership);
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(membership.MembershipId);

        Assert.NotNull(result);
        Assert.Equal("user-1", result!.UserId);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllMemberships()
    {
        using var context = TestDbContextFactory.Create();
        var repo = new MembershipRepository(context);

        context.Memberships.AddRange(
            new Membership { UserId = "u1", MembershipTypeId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), IsActive = true },
            new Membership { UserId = "u2", MembershipTypeId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), IsActive = true }
        );
        await context.SaveChangesAsync();

        var results = await repo.GetAllAsync();

        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task Delete_RemovesMembership()
    {
        using var context = TestDbContextFactory.Create();
        var repo = new MembershipRepository(context);

        var membership = new Membership
        {
            UserId = "u1",
            MembershipTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            IsActive = true
        };
        context.Memberships.Add(membership);
        await context.SaveChangesAsync();

        repo.Delete(membership);
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(membership.MembershipId);
        Assert.Null(result);
    }

    [Fact]
    public async Task Update_ModifiesMembership()
    {
        using var context = TestDbContextFactory.Create();
        var repo = new MembershipRepository(context);

        var membership = new Membership
        {
            UserId = "u1",
            MembershipTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            IsActive = true
        };
        context.Memberships.Add(membership);
        await context.SaveChangesAsync();

        membership.IsActive = false;
        repo.Update(membership);
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(membership.MembershipId);
        Assert.NotNull(result);
        Assert.False(result!.IsActive);
    }
}
