using Illyrian.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Illyrian.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestItemRepository _repo;

    public TestController(ITestItemRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTests()
    {
        var tests = await _repo.GetAllAsync();
        return Ok(tests);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTest(int id)
    {
        var test = await _repo.GetByIdAsync(id);
        if (test == null) return NotFound();
        return Ok(test);
    }
}
