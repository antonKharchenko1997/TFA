using Microsoft.AspNetCore.Mvc;
using TFA.Domain.UseCases.GetForums;
using Forum = TFA.API.Models.Forum;

namespace TFA.API.Controllers;

[ApiController]
[Route("forum")]
public class ForumController : ControllerBase
{
    /// <summary>
    /// Get list of every forum
    /// </summary>
    /// <param name="useCase"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(string[]))]
    public async Task<IActionResult> GetForums([FromServices] IGetForumsUseCase useCase, CancellationToken token)
    {
        var forums = await useCase.Execute(token);
        return Ok(forums.Select(i=>new Forum
        {
            Id = i.Id,
            Title = i.Title
        }));
    }
}