using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class RoleMenuConfiguration : IEntityTypeConfiguration<RoleMenu>
{
    public void Configure(EntityTypeBuilder<RoleMenu> builder)
    {
        builder.ToTable("RoleMenus");

        builder.HasKey(e => e.Id).HasName("PK__RoleMenu__3214EC072959D2D2");

        builder.HasIndex(e => e.CreatedBy, "IX_RoleMenus_CreatedBy");
        builder.HasIndex(e => e.MenuId, "IX_RoleMenus_MenuId");
        builder.HasIndex(e => e.ModifiedBy, "IX_RoleMenus_ModifiedBy");
        builder.HasIndex(e => e.RoleId, "IX_RoleMenus_RoleId");

        builder.Property(e => e.Created).HasColumnType("datetime");
        builder.Property(e => e.Modified).HasColumnType("datetime");

        builder.HasOne(d => d.Menu).WithMany(p => p.RoleMenus)
            .HasForeignKey(d => d.MenuId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_RoleMenus_MenuId");
    }
}
