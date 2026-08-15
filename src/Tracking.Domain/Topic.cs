using Tracking.Domain.Core;

namespace Tracking.Domain;

public sealed class Topic : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsArchived { get; set; }
    
    private Topic() {}

    public static Topic Create(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название темы не может быть пустым.");

        return new Topic()
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Description = description?.Trim()
        };
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название темы не может быть пустым.");

        Title = title.Trim();
    }

    public void Archive() => IsArchived = true;
    public void Restore() => IsArchived = false;
}