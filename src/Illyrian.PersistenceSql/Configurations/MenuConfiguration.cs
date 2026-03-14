using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menu");

        builder.HasKey(e => e.Id).HasName("PK__Menu__3214EC076025AA71");

        builder.HasIndex(e => e.CreatedBy, "IX_Menu_CreatedBy");
        builder.HasIndex(e => e.ModifiedBy, "IX_Menu_ModifiedBy");
        builder.HasIndex(e => e.ParentId, "IX_Menu_ParentId");

        builder.Property(e => e.Created).HasColumnType("datetime");
        builder.Property(e => e.Icon).HasMaxLength(100);
        builder.Property(e => e.Modified).HasColumnType("datetime");
        builder.Property(e => e.NameEn).HasMaxLength(100);
        builder.Property(e => e.NameSq).HasMaxLength(100);
        builder.Property(e => e.NameSr).HasMaxLength(100);
        builder.Property(e => e.Path).HasMaxLength(200);

        builder.HasOne(d => d.Parent).WithMany(p => p.Children)
            .HasForeignKey(d => d.ParentId)
            .HasConstraintName("FK_Menu_ParentId");
    }
}
