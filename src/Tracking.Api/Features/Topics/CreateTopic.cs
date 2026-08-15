using FluentValidation;
using Tracking.Api.Infrastructure;
using Tracking.Application.Core;
using Tracking.Domain;

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
        ITopicRepository topics,
        IUnitOfWork unitOfWork,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        if (await topics.ExistsByTitleAsync(request.Title, ct))
        {
            return Results.Problem(
                title: "Тема уже существует",
                detail: $"Тема с названием «{request.Title.Trim()}» уже заведена",
                statusCode: StatusCodes.Status409Conflict);
        }

        var topic = Topic.Create(request.Title, request.Description);
        await topics.AddAsync(topic, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Results.Created($"/api/v1/topics/{topic.Id}", new Response(topic.Id, topic.Title));
    }
}