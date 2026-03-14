using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A78599B1A25C56");

        builder.HasIndex(e => e.MembershipTypeId, "IX_Memberships_MembershipTypeID");
        builder.HasIndex(e => e.UserId, "IX_Memberships_UserID");

        builder.Property(e => e.MembershipId).HasColumnName("MembershipID");
        builder.Property(e => e.EndDate).HasColumnType("datetime");
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
        builder.Property(e => e.StartDate).HasColumnType("datetime");
        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.HasOne(d => d.MembershipType).WithMany(p => p.Memberships)
            .HasForeignKey(d => d.MembershipTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Membershi__Membe__73BA3083");
    }
}
