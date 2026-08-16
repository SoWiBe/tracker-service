using FluentValidation;
using Tracking.Api.Extensions;
using Tracking.Api.Infrastructure;
using Tracking.Application.Topics.CreateTopic;

namespace Tracking.Api.Features.Topics;

public sealed class CreateTopic : IEndpoint
{
    public sealed record Request(string Title, string? Description);
    public sealed record Response(Guid Id, string Title);

    public void MapEndpoint(IEndpointRouteBuilder app)
        => app.MapPost("topics", HandleAsync)
            .WithName(nameof(CreateTopic))
            .WithTags("Topics")
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();
            

    private static async Task<IResult> HandleAsync(
        Request request,
        IValidator<Request> validator,
        CreateTopicHandler handler,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler
            .HandleAsync(new CreateTopicCommand(request.Title, request.Description), ct);

        return result.Match(
            topic => Results.Created($"/api/v1/topics/{topic.Id}", new Response(topic.Id, topic.Title)),
             errors => errors.ToProblem());
    }

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}