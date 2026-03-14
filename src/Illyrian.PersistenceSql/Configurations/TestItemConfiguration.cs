using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class TestItemConfiguration : IEntityTypeConfiguration<TestItem>
{
    public void Configure(EntityTypeBuilder<TestItem> builder)
    {
        builder.ToTable("Test");

        builder.HasKey(e => e.TestId).HasName("PK__Test__8CC33100F58A8C40");

        builder.Property(e => e.TestId).HasColumnName("TestID");
        builder.Property(e => e.Description)
            .HasMaxLength(255)
            .IsUnicode(false);
    }
}
