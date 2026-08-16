using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Tracking.Application.Core;
using Tracking.Application.Core.Handlers;
using Tracking.Application.Core.Repositories;
using Tracking.Application.Topics.CreateTopic;
using Tracking.Application.Topics.GetTopics;
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

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTopicHandler>();
        services.AddScoped<GetTopicsHandler>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName!.Replace('+', '.'));
        });

        return services;
    }
}