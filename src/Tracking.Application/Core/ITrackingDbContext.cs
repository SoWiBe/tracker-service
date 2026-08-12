using Microsoft.EntityFrameworkCore;
using Tracking.Domain;

namespace Tracking.Application.Core;

public interface ITrackingDbContext
{
    DbSet<Topic> Topics { get; }
    DbSet<TrackSession> TrackSessions { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}