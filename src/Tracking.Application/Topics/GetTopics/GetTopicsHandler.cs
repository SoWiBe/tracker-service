using ErrorOr;

using Tracking.Application.Core.Handlers;
using Tracking.Application.Core.Repositories;

namespace Tracking.Application.Topics.GetTopics;

public sealed class GetTopicsHandler(ITopicRepository topics) 
    : IQueryHandler<GetTopicsQuery, IReadOnlyList<TopicResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<TopicResponse>>> HandleAsync(
        GetTopicsQuery query, 
        CancellationToken ct = default)
    {
        var result = await topics.GetAllAsync(query.IncludedArchived, ct);

        return result
            .Select(x => new TopicResponse(x.Id, x.Title, x.Description, x.IsArchived, x.CreatedAt))
            .ToList();
    }
}