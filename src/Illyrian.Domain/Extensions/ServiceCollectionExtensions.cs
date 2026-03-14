using System.Text;
using Illyrian.Domain.Configuration;
using Illyrian.Domain.Services.Auth;
using Illyrian.Domain.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Illyrian.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(options =>
        {
            options.Secret = configuration["JWT:Secret"]!;
            options.ValidIssuer = configuration["JWT:ValidIssuer"]!;
            options.ValidAudience = configuration["JWT:ValidAudience"]!;
            options.ExpirationInHours = 3;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
            };
        });

        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}
