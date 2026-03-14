using Illyrian.Domain.Repositories;
using Illyrian.Persistence.General;
using Illyrian.PersistenceSql.Context;
using Illyrian.PersistenceSql.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Illyrian.PersistenceSql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceSql(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IllyrianDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<IllyrianDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.SlidingExpiration = true;
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<IllyrianDbContext>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IGymClassRepository, GymClassRepository>();
        services.AddScoped<IUserClassRepository, UserClassRepository>();
        services.AddScoped<IUserScheduleRepository, UserScheduleRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<ITestItemRepository, TestItemRepository>();

        return services;
    }
}
