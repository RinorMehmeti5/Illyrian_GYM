using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class UserScheduleConfiguration : IEntityTypeConfiguration<UserSchedule>
{
    public void Configure(EntityTypeBuilder<UserSchedule> builder)
    {
        builder.ToTable("UsersSchedule");

        builder.HasKey(e => e.UserScheduleId).HasName("PK__UsersSch__9659B0319F77889C");

        builder.HasIndex(e => e.ScheduleId, "IX_UsersSchedule_ScheduleID");
        builder.HasIndex(e => e.UserId, "IX_UsersSchedule_UserID");

        builder.Property(e => e.UserScheduleId).HasColumnName("UserScheduleID");
        builder.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.HasOne(d => d.Schedule).WithMany(p => p.UserSchedules)
            .HasForeignKey(d => d.ScheduleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__UsersSche__Sched__31B762FC");
    }
}
