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

    public static TrackSession Log(
        Guid topicId,
        DateOnly date,
        int minutes,
        string? notes = null
    )
    {
        if (topicId == Guid.Empty)
            throw new ArgumentException("Не указана тема.", nameof(topicId));

        if (minutes is <= 0 or > 24 * 60)
            throw new ArgumentOutOfRangeException(
                nameof(minutes), minutes, "Длительность должна быть от 1 до 1440 минут.");

        if (notes is { Length: > 2000 })
            throw new ArgumentException("Заметка длиннее 2000 символов.", nameof(notes));

        var anchor = new DateTimeOffset(date.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);

        return new TrackSession
        {
            TopicId = topicId,
            Date = date,
            StartedAt = anchor,
            EndedAt = anchor,
            DurationMinutes = minutes,
            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()
        };
    }

    public bool IsRunning => EndedAt is null;
    public bool IsManual => EndedAt == StartedAt;
}