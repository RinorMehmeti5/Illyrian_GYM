using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Language");

        builder.HasKey(e => e.LanguageId).HasName("PK__Languge__B938558BB0A1FDCF");

        builder.Property(e => e.LanguageId).HasColumnName("LanguageID");
        builder.Property(e => e.NameEn)
            .HasMaxLength(100)
            .HasColumnName("NameEN");
        builder.Property(e => e.NameSq)
            .HasMaxLength(50)
            .HasColumnName("NameSQ");
        builder.Property(e => e.Notes).HasMaxLength(255);
    }
}
