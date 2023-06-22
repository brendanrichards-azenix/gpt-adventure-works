using GptAdventureWorks.Web.Data;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace GptAdventureWorks.Web.SemanticKernel;

public static class SemanticKernelExtensions
{
    public static IServiceCollection AddSemanticKernelServices(this IServiceCollection services)
    {
        // Semantic Kernel
        services.AddScoped<IKernel>(sp =>
        {
            var openAiConfig = sp.GetRequiredService<IOptions<OpenAiConfig>>();
            IKernel kernel = Kernel.Builder
                .WithLogger(sp.GetRequiredService<ILogger<IKernel>>())
                //.WithMemory(sp.GetRequiredService<ISemanticTextMemory>())
                .WithAzureChatCompletionService("gpt3-5", openAiConfig.Value.Endpoint, openAiConfig.Value.Key)
                .Build();

            kernel.ImportSkill(new SqlServerSkill(sp.GetRequiredService<DbConfig>()));

            return kernel;
        });
        

        return services;
    }
}