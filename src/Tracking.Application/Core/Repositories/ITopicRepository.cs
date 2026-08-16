using Tracking.Domain;

namespace Tracking.Application.Core.Repositories;

public interface ITopicRepository
{
    Task AddAsync(Topic topic, CancellationToken ct = default);
    Task<Topic?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsByTitleAsync(string title, CancellationToken ct = default);
    Task<IReadOnlyList<Topic>> GetAllAsync(bool includeArchived = false, CancellationToken ct = default);
}