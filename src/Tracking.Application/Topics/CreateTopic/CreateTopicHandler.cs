using ErrorOr;
using Tracking.Application.Core;
using Tracking.Application.Core.Repositories;
using Tracking.Domain;
using Tracking.Domain.Topics;

namespace Tracking.Application.Topics.CreateTopic;

public class CreateTopicHandler(
    ITopicRepository topics,
    IUnitOfWork unitOfWork)
{
    public async Task<ErrorOr<Topic>> HandleAsync(CreateTopicCommand command, CancellationToken ct = default)
    {
        var title = command.Title.Trim();
        if (await topics.ExistsByTitleAsync(title, ct))
            return TopicErrors.DuplicateTitle(title);

        var result = Topic.Create(title, command.Description);
        if (result.IsError)
            return result.Errors;

        var topic = result.Value;

        await topics.AddAsync(topic, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return topic;
    }
}