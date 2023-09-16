using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFA.Storage;

namespace TFA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ForumController : ControllerBase
{
    /// <summary>
    /// Get list of every forum
    /// </summary>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(string[]))]
    public async Task<IActionResult> GetForums([FromServices] ForumDbContext dbContext)
    {
        await dbContext.Forums.Select(f => f.Title).ToListAsync();
        return Ok();
    }
}