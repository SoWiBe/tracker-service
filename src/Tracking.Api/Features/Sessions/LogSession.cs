using FluentValidation;
using Tracking.Api.Infrastructure;
using Tracking.Application.Core;
using Tracking.Domain;

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


    private static async Task<IResult> HandleAsync(
        Request request,
        IValidator<Request> validator,
        ISessionRepository sessions,
        IUnitOfWork unitOfWork,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        var session = TrackSession.Log(
            request.TopicId,
            request.Date,
            request.Minutes,
            request.Notes
        );

        await sessions.AddAsync(session, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Results.Created($"/api/v1/sessions/{session.Id}", 
            new Response(session.Id));
    }

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TopicId).NotEmpty();
            RuleFor(x => x.Minutes).InclusiveBetween(1, 24 * 60);
            RuleFor(x => x.Notes).MaximumLength(2000);
        }
    }
}