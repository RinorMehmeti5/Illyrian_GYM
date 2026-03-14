using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class MembershipTypeConfiguration : IEntityTypeConfiguration<MembershipType>
{
    public void Configure(EntityTypeBuilder<MembershipType> builder)
    {
        builder.ToTable("MembershipTypes");

        builder.HasKey(e => e.MembershipTypeId).HasName("PK__Membersh__F35A3E5965F0D49D");

        builder.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");
    }
}
