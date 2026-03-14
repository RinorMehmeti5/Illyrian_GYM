using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class GymClassConfiguration : IEntityTypeConfiguration<GymClass>
{
    public void Configure(EntityTypeBuilder<GymClass> builder)
    {
        builder.ToTable("Classes");

        builder.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927A02A82E515");

        builder.Property(e => e.ClassId).HasColumnName("ClassID");
        builder.Property(e => e.ClassName).HasMaxLength(100);
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.ScheduleDay).HasMaxLength(10);
    }
}
