using Tracking.Application.Core;

namespace Tracking.Infrastructure.Persistence;

internal sealed class UnitOfWork(TrackingDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}