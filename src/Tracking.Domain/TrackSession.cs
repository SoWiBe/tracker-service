using Tracking.Domain.Core;

namespace Tracking.Domain;

// запись трека сессии для конкретного топика (Topic)
public sealed class TrackSession : BaseEntity
{
    public Guid TopicId { get; private set; } // что трекаем
    public DateOnly Date { get; private set; }  // дата
    public DateTimeOffset StartedAt { get; private set; } // когда начали
    public DateTimeOffset? EndedAt { get; private set; } // когда закончили
    public int DurationMinutes { get; private set; } // общее время
    public string? Notes { get; private set; } // временные записи

    private TrackSession() { }
}