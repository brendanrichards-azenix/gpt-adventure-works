using GptAdventureWorks.Web.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GptAdventureWorks.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkChatController: ControllerBase
{
    private readonly IMediator _mediator;

    public SkChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SkChatResponse>> Post([FromBody] SkChat chatQuery)
    {
        return Ok(await _mediator.Send(chatQuery));
    }
    
}