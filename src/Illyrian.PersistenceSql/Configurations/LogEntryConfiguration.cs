using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
{
    public void Configure(EntityTypeBuilder<LogEntry> builder)
    {
        builder.ToTable("Logs");

        builder.HasKey(e => e.Id).HasName("PK__Logs__3214EC07EB0E13EF");

        builder.HasIndex(e => e.UserId, "IX_Logs_UserId");

        builder.Property(e => e.Action).HasMaxLength(100);
        builder.Property(e => e.Controller).HasMaxLength(100);
        builder.Property(e => e.HttpMethod).HasMaxLength(10);
        builder.Property(e => e.InsertedDate).HasColumnType("datetime");
        builder.Property(e => e.Ip).HasMaxLength(50);
        builder.Property(e => e.Url).HasMaxLength(200);
    }
}
