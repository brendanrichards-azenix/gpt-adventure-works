using GptAdventureWorks.Web.Application;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GptAdventureWorks.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{

    private readonly IMediator _mediator;

    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatQuery chatQuery)
    {
        return Ok(await _mediator.Send(chatQuery));
    }
    
    [HttpGet("GetDbColumns")]
    public async Task<ActionResult> GetDbColumns()
    {
        string json = await _mediator.Send(new GetDbColumns());
        return new ContentResult()
        {
            Content = json,
            ContentType = "application/json"
        };
    }
    
}