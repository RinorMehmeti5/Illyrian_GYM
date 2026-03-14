namespace Illyrian.Domain.Services.User;

public interface ICurrentUserService
{
    string? UserId { get; }
    bool IsAuthenticated { get; }
    string? UserName { get; }
}
