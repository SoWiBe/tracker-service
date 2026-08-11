using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracking.Domain;

namespace Tracking.Infrastructure.Configurations;

public sealed class TrackSessionConfiguration : IEntityTypeConfiguration<TrackSession>
{
    public void Configure(EntityTypeBuilder<TrackSession> b)
    {
        b.ToTable("track_sessions");
        b.HasKey(x => x.Id);

        b.Property(x => x.Date).HasColumnType("date");
        b.Property(x => x.StartedAt).HasColumnType("timestampz");
        b.Property(x => x.EndedAt).HasColumnType("timestampz");
        b.Property(x => x.DurationMinutes).IsRequired();
        b.Property(x => x.Notes).HasMaxLength(2000);

        b.HasOne<Topic>()
            .WithMany()
            .HasForeignKey(x => x.TopicId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => new { x.TopicId, x.Date });

        b.HasIndex(x => x.TopicId)
            .HasFilter("\"EndedAt\" IS NULL")
            .IsUnique();
    }
}