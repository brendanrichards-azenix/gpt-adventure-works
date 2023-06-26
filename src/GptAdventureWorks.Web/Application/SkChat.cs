using System.Text.Json;
using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;

namespace GptAdventureWorks.Web.Application;

public class SkChat: IRequest<SkChatResponse>
{
    public string Question { get; set; } = string.Empty;

    public IEnumerable<KeyValuePair<string, string>> Variables { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
}

public class SkChatResponse
{
    public string Value { get; set; } = string.Empty;

    public KeyValuePair<string, string>? Variables { get; set; } = new KeyValuePair<string, string>();
}


public class SkChatHandler : IRequestHandler<SkChat, SkChatResponse>
{
    private readonly IKernel _kernel;
    private readonly ILogger _logger;

    public SkChatHandler(IKernel kernel, ILogger<SkChatHandler> logger)
    {
        _kernel = kernel;
        _logger = logger;
    }

    public async Task<SkChatResponse> Handle(SkChat request, CancellationToken cancellationToken)
    {
        var planner = new SequentialPlanner(_kernel);
        var plan = await planner.CreatePlanAsync("which database tables contain a column called 'name'?");
        _logger.LogInformation("Plan: {Plan}", JsonSerializer.Serialize(plan, new JsonSerializerOptions(){ WriteIndented = true}));

        var result = await plan.InvokeAsync(logger: _logger, cancellationToken: cancellationToken);

        return new SkChatResponse()
        {
            Value = result.Result,
            //Variables = result.Variables.ToDictionary();
        };
    }
}