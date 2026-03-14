using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class StatusTypeConfiguration : IEntityTypeConfiguration<StatusType>
{
    public void Configure(EntityTypeBuilder<StatusType> builder)
    {
        builder.ToTable("StatusType");

        builder.HasKey(e => e.StatusTypeId).HasName("PK__StatusTy__A84F3C734D3330DE");

        builder.HasIndex(e => e.InsertedFrom, "IX_StatusType_InsertedFrom");
        builder.HasIndex(e => e.UpdatedFrom, "IX_StatusType_UpdatedFrom");

        builder.Property(e => e.InsertedDate).HasColumnType("datetime");
        builder.Property(e => e.NameEn).HasMaxLength(100);
        builder.Property(e => e.NameSq).HasMaxLength(100);
        builder.Property(e => e.NameSr).HasMaxLength(100);
        builder.Property(e => e.UpdatedDate).HasColumnType("datetime");
    }
}
