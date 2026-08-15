using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Tracking.Domain;

namespace Tracking.Infrastructure.Configurations;

public sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> b)
    {
        b.ToTable("topics");
        b.HasKey(x => x.Id);

        b.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(x => x.Description)
            .HasMaxLength(1000);

        b.Property(x => x.CreatedAt)
            .HasColumnType("timestamptz")
            .IsRequired();

        b.Property(x => x.UpdatedAt)
            .HasColumnType("timestamptz");

        b.HasIndex(x => x.Title)
            .IsUnique()
            .HasDatabaseName("ix_topics_title_lower");
    }
}