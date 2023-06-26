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
            var openAiConfig = sp.GetRequiredService<OpenAiConfig>();
            if (string.IsNullOrEmpty(openAiConfig.Endpoint) || string.IsNullOrEmpty(openAiConfig.Key))
            {
                throw new ApplicationException("missing OpenAI configuration");
            }
            
            IKernel kernel = Kernel.Builder
                .WithLogger(sp.GetRequiredService<ILogger<IKernel>>())
                //.WithMemory(sp.GetRequiredService<ISemanticTextMemory>())
                .WithAzureChatCompletionService("gpt-35-turbo", openAiConfig.Endpoint, openAiConfig.Key)
                .Build();

            kernel.ImportSkill(new SqlServerSkill(sp.GetRequiredService<DbConfig>()));

            return kernel;
        });

        return services;
    }
}