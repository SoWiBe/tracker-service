
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tracking.Domain.Core;

namespace Tracking.Infrastructure.Interceptors;

public sealed class AuditableInterceptor(TimeProvider clock) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        var now = clock.GetUtcNow();

        foreach (var entry in eventData.Context!.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = now;  
            if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = now;  
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}