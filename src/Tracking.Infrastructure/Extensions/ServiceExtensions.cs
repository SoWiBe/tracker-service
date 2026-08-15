using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tracking.Application.Core;
using Tracking.Infrastructure.Interceptors;
using Tracking.Infrastructure.Persistence;
using Tracking.Infrastructure.Persistence.Repositories;

namespace Tracking.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<AuditableInterceptor>();

        services.AddDbContext<TrackingDbContext>((sp, o) => 
            o.UseNpgsql(configuration.GetConnectionString("Postgres"))
            .AddInterceptors(sp.GetRequiredService<AuditableInterceptor>()));

        services.AddScoped<ITrackingDbContext>(sp => sp.GetRequiredService<TrackingDbContext>());

        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}