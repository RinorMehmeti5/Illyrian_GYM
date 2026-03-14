namespace Illyrian.Persistence.Exercise;

public class ExerciseDto
{
    public int ExerciseId { get; set; }
    public string ExerciseName { get; set; } = null!;
    public string? Description { get; set; }
    public string? MuscleGroup { get; set; }
    public string? DifficultyLevel { get; set; }
}
