using Azure;
using Azure.AI.OpenAI;
using GptAdventureWorks.Web.Data;
using MediatR;

namespace GptAdventureWorks.Web.Application;

public class ChatQuery: IRequest<ChatResponse>
{
    public string? Question { get; set; }
}

public class ChatResponse
{
    public string? Response { get; set; }
}

public class ChatQueryHandler : IRequestHandler<ChatQuery, ChatResponse>
{
    private readonly DbConfig _dbConfig;
    private readonly OpenAiConfig _openAiConfig;
    private readonly IMediator _mediator;

    public ChatQueryHandler(DbConfig dbConfig, OpenAiConfig openAiConfig, IMediator mediator)
    {
        _dbConfig = dbConfig;
        _openAiConfig = openAiConfig;
        _mediator = mediator;
    }

    public async Task<ChatResponse> Handle(ChatQuery request, CancellationToken cancellationToken)
    {
        var columnsJson = await _mediator.Send(new GetDbColumns(), cancellationToken);
        string systemPrompt = @$"
Here are the columns in your database: 
{columnsJson}
Generate an SQL query for every following question using the information about my database.
Return only the SQL.
";
        
        var chatOptions = new ChatCompletionsOptions();
        chatOptions.Messages.Add(new ChatMessage(ChatRole.System, systemPrompt));
        chatOptions.Messages.Add(new ChatMessage(ChatRole.User, request.Question!));
        
        var openAiClient = new OpenAIClient(new Uri(_openAiConfig.Endpoint!), new AzureKeyCredential(_openAiConfig.Key!));
        var response = await openAiClient.GetChatCompletionsAsync("gpt3-5", chatOptions, cancellationToken);

        return new ChatResponse()
        {
            Response = response.Value.Choices[0].Message.Content
        };

    }
}