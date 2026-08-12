using Microsoft.EntityFrameworkCore;
using Tracking.Application.Core;
using Tracking.Domain;

namespace Tracking.Infrastructure.Persistence;

public sealed class TrackingDbContext(DbContextOptions<TrackingDbContext> options) 
    : DbContext(options), ITrackingDbContext
{
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<TrackSession> TrackSessions => Set<TrackSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}