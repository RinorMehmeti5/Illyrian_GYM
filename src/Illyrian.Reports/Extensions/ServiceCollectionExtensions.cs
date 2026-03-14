using Microsoft.Extensions.DependencyInjection;

namespace Illyrian.Reports.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReports(this IServiceCollection services)
    {
        return services;
    }
}
