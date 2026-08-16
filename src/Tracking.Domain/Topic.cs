using ErrorOr;
using Tracking.Domain.Core;
using Tracking.Domain.Topics;

namespace Tracking.Domain;

public sealed class Topic : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsArchived { get; set; }
    
    private Topic() {}

    public static ErrorOr<Topic> Create(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title)) return TopicErrors.EmptyTitle;

        return new Topic()
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description?.Trim()
        };
    }

    public ErrorOr<Success> Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return TopicErrors.EmptyTitle;

        Title = title.Trim();
        return Result.Success;
    }

    public void Archive() => IsArchived = true;
    public void Restore() => IsArchived = false;
}