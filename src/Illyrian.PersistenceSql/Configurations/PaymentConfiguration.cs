using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A589CE31E65");

        builder.HasIndex(e => e.MembershipId, "IX_Payments_MembershipID");
        builder.HasIndex(e => e.UserId, "IX_Payments_UserID");

        builder.Property(e => e.PaymentId).HasColumnName("PaymentID");
        builder.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.MembershipId).HasColumnName("MembershipID");
        builder.Property(e => e.Notes).HasMaxLength(255);
        builder.Property(e => e.PaymentDate).HasColumnType("datetime");
        builder.Property(e => e.PaymentMethod).HasMaxLength(50);
        builder.Property(e => e.TransactionId)
            .HasMaxLength(100)
            .HasColumnName("TransactionID");
        builder.Property(e => e.UserId).HasColumnName("UserID");

        builder.HasOne(d => d.Membership).WithMany(p => p.Payments)
            .HasForeignKey(d => d.MembershipId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__Payments__Member__778AC167");
    }
}
