using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planning;

namespace GptAdventureWorks.Web.Application;

public class SkChat: IRequest<SkChatResponse>
{
    public string Input { get; set; } = string.Empty;

    public IEnumerable<KeyValuePair<string, string>> Variables { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
}

public class SkChatResponse
{
    public string Value { get; set; } = string.Empty;

    public IEnumerable<KeyValuePair<string, string>>? Variables { get; set; } = Enumerable.Empty<KeyValuePair<string, string>>();
}


public class SkChatHandler : IRequestHandler<SkChat, SkChatResponse>
{
    private readonly IKernel _kernel;

    public SkChatHandler(IKernel kernel)
    {
        _kernel = kernel;
    }

    public async Task<SkChatResponse> Handle(SkChat request, CancellationToken cancellationToken)
    {
        var planner = new SequentialPlanner(_kernel);
        var plan = await planner.CreatePlanAsync("which database tables contains a name column?");
    }
}