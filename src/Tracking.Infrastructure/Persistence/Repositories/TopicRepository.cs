using Microsoft.EntityFrameworkCore;
using Tracking.Application.Core;
using Tracking.Domain;

namespace Tracking.Infrastructure.Persistence.Repositories;

internal sealed class TopicRepository(TrackingDbContext context) : ITopicRepository
{
    public async Task AddAsync(Topic topic, CancellationToken ct = default)
        => await context.Topics.AddAsync(topic, ct);

    public async Task<Topic?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Topics.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<bool> ExistsByTitleAsync(string title, CancellationToken ct = default)
        => await context.Topics.AnyAsync(t => t.Title.ToLower() == title.Trim().ToLower(), ct);

    public async Task<IReadOnlyList<Topic>> GetAllAsync(bool includeArchived = false, CancellationToken ct = default)
        => await context.Topics
            .AsNoTracking()
            .Where(t => includeArchived || !t.IsArchived)
            .OrderBy(t => t.Title)
            .ToListAsync(ct);
}