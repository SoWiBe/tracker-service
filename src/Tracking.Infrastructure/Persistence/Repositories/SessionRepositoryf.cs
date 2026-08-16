using Microsoft.EntityFrameworkCore;
using Tracking.Application.Core;
using Tracking.Application.Core.Repositories;
using Tracking.Domain;
using Tracking.Domain.Core;

namespace Tracking.Infrastructure.Persistence.Repositories;

internal sealed class SessionRepository(TrackingDbContext context)
    : ISessionRepository
{
    public async Task AddAsync(TrackSession session, CancellationToken ct = default)
        => await context.TrackSessions.AddAsync(session, ct);

    public async Task<TrackSession?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.TrackSessions.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<TrackSession?> GetRunningAsync(Guid topicId, CancellationToken ct = default)
        => await context.TrackSessions.FirstOrDefaultAsync(x => x.TopicId == topicId && x.EndedAt == null, ct);
}