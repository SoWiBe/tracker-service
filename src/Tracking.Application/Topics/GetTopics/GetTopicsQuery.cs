namespace Tracking.Application.Topics.GetTopics;

public sealed record GetTopicsQuery(bool IncludedArchived = false);