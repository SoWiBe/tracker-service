namespace Tracking.Application.Topics.GetTopics;

public sealed record TopicResponse(
    Guid Id,
    string Title,
    string? Description,
    bool IsArchived,
    DateTimeOffset CreatedAt);