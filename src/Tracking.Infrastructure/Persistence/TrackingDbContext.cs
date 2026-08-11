using Microsoft.EntityFrameworkCore;
using Tracking.Domain;

namespace Tracking.Ifrastructure.Persistence;

public sealed class TrackingDbContext(DbContextOptions<TrackingDbContext> options) : DbContext(options)
{
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<TrackSession> TrackSessions => Set<TrackSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}