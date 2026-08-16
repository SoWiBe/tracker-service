using Tracking.Api.Extensions;
using Tracking.Api.Infrastructure;
using Tracking.Application.Topics.GetTopics;

namespace Tracking.Api.Features.Topics;

public sealed class GetTopics : IEndpoint
{
    public sealed record Response(IReadOnlyList<TopicResponse> Topics);

    public void MapEndpoint(IEndpointRouteBuilder app)
        => app.MapGet("topics", HandleAsync)
            .WithName(nameof(GetTopics))
            .WithTags("Topics")
            .Produces<Response>(StatusCodes.Status200OK);

    private static async Task<IResult> HandleAsync(
        GetTopicsHandler handler,
        CancellationToken ct,
        bool includedArchived = false)
    {
        var result = await handler.HandleAsync(new GetTopicsQuery(includedArchived), ct);

        return result.Match(
            topics => Results.Ok(new Response(result.Value)),
             errors => errors.ToProblem());
    }
}