using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IllyrianAPI.Data.General;
using IllyrianAPI.Data.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace IllyrianAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public TestController(
            IllyrianContext db,
            UserManager<ApplicationUser> userManager
        ) : base(db, userManager)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests()
        {
            return await _db.Test.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await _db.Test.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return test;
        }
    }
}