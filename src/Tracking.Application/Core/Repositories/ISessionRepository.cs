using Tracking.Domain;

namespace Tracking.Application.Core.Repositories;

public interface ISessionRepository
{
    Task AddAsync(TrackSession session, CancellationToken ct = default);
    Task<TrackSession?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TrackSession?> GetRunningAsync(Guid topicId, CancellationToken ct = default);
}