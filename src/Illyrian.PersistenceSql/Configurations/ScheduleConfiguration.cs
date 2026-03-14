using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedule");

        builder.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B69AC6F8FB5");

        builder.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
        builder.Property(e => e.DayOfWeek).HasMaxLength(10);
        builder.Property(e => e.EndTime).HasColumnType("datetime");
        builder.Property(e => e.StartTime).HasColumnType("datetime");
    }
}
