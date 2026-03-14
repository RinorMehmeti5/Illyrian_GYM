using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.Exercise;
using Illyrian.RestApi.Controllers;
using Illyrian.RestApi.Tests.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Illyrian.RestApi.Tests.Controllers;

public class ExerciseControllerTests
{
    private readonly Mock<IExerciseRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<ExercisesController>> _loggerMock;
    private readonly ExercisesController _controller;

    public ExerciseControllerTests()
    {
        _repoMock = new Mock<IExerciseRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<ExercisesController>>();
        _controller = new ExercisesController(_repoMock.Object, _unitOfWorkMock.Object, AutoMapperFixture.Mapper, _loggerMock.Object);
    }

    [Fact]
    public async Task GetExercises_ReturnsOkWithList()
    {
        var exercises = new List<Exercise>
        {
            new() { ExerciseId = 1, ExerciseName = "Squat", MuscleGroup = "Legs", DifficultyLevel = "Intermediate" },
            new() { ExerciseId = 2, ExerciseName = "Bench Press", MuscleGroup = "Chest", DifficultyLevel = "Intermediate" }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(exercises);

        var result = await _controller.GetExercises();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetExercise_ExistingId_ReturnsOk()
    {
        var exercise = new Exercise { ExerciseId = 1, ExerciseName = "Squat", MuscleGroup = "Legs" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exercise);

        var result = await _controller.GetExercise(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetExercise_NonExistingId_ReturnsNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Exercise?)null);

        var result = await _controller.GetExercise(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostExercise_ValidData_ReturnsCreated()
    {
        var dto = new ExerciseDto
        {
            ExerciseName = "Deadlift",
            Description = "Compound movement",
            MuscleGroup = "Back",
            DifficultyLevel = "Advanced"
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Exercise>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(0);

        var result = await _controller.PostExercise(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.NotNull(createdResult.Value);
    }

    [Fact]
    public async Task DeleteExercise_ExistingId_ReturnsNoContent()
    {
        var exercise = new Exercise { ExerciseId = 1, ExerciseName = "Squat" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(exercise);

        var result = await _controller.DeleteExercise(1);

        Assert.IsType<NoContentResult>(result);
        _repoMock.Verify(r => r.Delete(exercise), Times.Once);
    }
}
