using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Tracking.Ifrastructure.Persistence;

namespace Tracking.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TrackingDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("Postgres")));
    }
}