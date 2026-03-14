using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class UserClassConfiguration : IEntityTypeConfiguration<UserClass>
{
    public void Configure(EntityTypeBuilder<UserClass> builder)
    {
        builder.ToTable("UserClasses");

        builder.HasKey(e => e.UserClassId).HasName("PK__UserClas__151870CF1013FE44");

        builder.HasIndex(e => e.ClassId, "IX_UserClasses_ClassID");
        builder.HasIndex(e => e.UserId, "IX_UserClasses_UserID");

        builder.Property(e => e.UserClassId).HasColumnName("UserClassID");
        builder.Property(e => e.ClassId).HasColumnName("ClassID");
        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.HasOne(d => d.Class).WithMany(p => p.UserClasses)
            .HasForeignKey(d => d.ClassId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__UserClass__Class__7D439ABD");
    }
}
