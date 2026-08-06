using Tracking.Domain.Core;

namespace Tracking.Domain;

public sealed class Topic : BaseEntity
{
    public string Title { get; private set; } = null!;
}