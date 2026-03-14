using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Services.Auth;

public interface ITokenService
{
    Task<(string Token, DateTime Expiration)> GenerateTokenAsync(Entities.User user, IList<string> roles);
}
