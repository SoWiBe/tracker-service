using ErrorOr;

namespace Tracking.Domain.Topics;

public static class TopicErrors
{
    public static Error DuplicateTitle(string title) => Error.Conflict(
        code: "Topic.DuplicateTitle",
        description: $"Тема с названием {title} уже заведена.");

    public static Error NotFound(Guid id) => Error.NotFound(
        code: "Topic.NotFound",
        description: "Тема {id} не найдена");

    public static Error EmptyTitle => Error.Validation(
        code: "Topic.EmptyTitle",
        description: "Название темы не может быть пустым");
}