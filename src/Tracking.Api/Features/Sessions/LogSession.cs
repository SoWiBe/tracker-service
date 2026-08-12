using FluentValidation;
using Tracking.Api.Infrastructure;

namespace Tracking.Api.Features.Sessions;

public sealed class LogSession : IEndpoint
{
    public sealed record Request(Guid TopicId, DateOnly Date, int Minutes, string? Notes);
    public sealed record Response(Guid Id);

    public void MapEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("sessions", HandleAsync)
            .WithName(nameof(LogSession))
            .WithTags("Sessions")
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();


    private static async void HandleAsync(
        Request request,
        IValidator<Request> validator,
        CancellationToken cancellationToken
    )
    {
        // TODO: add unitofwork and first repository + service
    }
}