using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Tracking.Api.Infrastructure;

namespace Tracking.Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] descriptors = [.. assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } 
                        && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))];

        services.TryAddEnumerable(descriptors);
        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? group = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        IEndpointRouteBuilder buidler = group is null ? app : group;

        foreach (IEndpoint endpoint in endpoints) endpoint.MapEndpoint(buidler);

        return app;
    }
}