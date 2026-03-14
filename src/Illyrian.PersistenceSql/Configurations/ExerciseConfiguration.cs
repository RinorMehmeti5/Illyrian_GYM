using Illyrian.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Illyrian.PersistenceSql.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("Exercises");

        builder.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F48FEF79A");

        builder.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.DifficultyLevel).HasMaxLength(20);
        builder.Property(e => e.ExerciseName).HasMaxLength(100);
        builder.Property(e => e.MuscleGroup).HasMaxLength(50);
    }
}
