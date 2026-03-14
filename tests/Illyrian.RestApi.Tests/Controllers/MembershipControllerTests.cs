using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.Membership;
using Illyrian.RestApi.Controllers;
using Illyrian.RestApi.Tests.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Illyrian.RestApi.Tests.Controllers;

public class MembershipControllerTests
{
    private readonly Mock<IMembershipRepository> _membershipRepoMock;
    private readonly Mock<IMembershipTypeRepository> _membershipTypeRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<MembershipController>> _loggerMock;
    private readonly MembershipController _controller;

    public MembershipControllerTests()
    {
        _membershipRepoMock = new Mock<IMembershipRepository>();
        _membershipTypeRepoMock = new Mock<IMembershipTypeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<MembershipController>>();
        _controller = new MembershipController(
            _membershipRepoMock.Object,
            _membershipTypeRepoMock.Object,
            _unitOfWorkMock.Object,
            AutoMapperFixture.Mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetMemberships_ReturnsOkWithList()
    {
        var memberships = new List<Membership>
        {
            new() { MembershipId = 1, UserId = "u1", MembershipTypeId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(30), IsActive = true, MembershipType = new MembershipType { Name = "Basic", DurationInDays = 30, Price = 29.99m } },
            new() { MembershipId = 2, UserId = "u2", MembershipTypeId = 2, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(90), IsActive = true, MembershipType = new MembershipType { Name = "Premium", DurationInDays = 90, Price = 79.99m } }
        };

        _membershipRepoMock.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(memberships);

        var result = await _controller.GetMemberships();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetMembership_ExistingId_ReturnsOk()
    {
        var membership = new Membership
        {
            MembershipId = 1,
            UserId = "u1",
            MembershipTypeId = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
            IsActive = true,
            MembershipType = new MembershipType { Name = "Basic", DurationInDays = 30, Price = 29.99m }
        };

        _membershipRepoMock.Setup(r => r.GetWithDetailsAsync(1)).ReturnsAsync(membership);

        var result = await _controller.GetMembership(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetMembership_NonExistingId_ReturnsNotFound()
    {
        _membershipRepoMock.Setup(r => r.GetWithDetailsAsync(999)).ReturnsAsync((Membership?)null);

        var result = await _controller.GetMembership(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteMembership_ExistingId_ReturnsNoContent()
    {
        var membership = new Membership { MembershipId = 1 };
        _membershipRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(membership);

        var result = await _controller.DeleteMembership(1);

        Assert.IsType<NoContentResult>(result);
        _membershipRepoMock.Verify(r => r.Delete(membership), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}
