using Microsoft.SemanticKernel;

namespace AzureExcelChat.Console.Utility;

public static class KernelConstruction
{
    public static Kernel Create(string endpoint, string apiKey, string deploymentName)
    {
        var builder = Kernel.CreateBuilder();

        builder.AddAzureOpenAIChatCompletion(
            deploymentName: deploymentName,
            endpoint: endpoint,
            apiKey: apiKey
        );

        return builder.Build();
    }

    public static KernelFunction CreateFunction(
        Kernel kernel, string promptTemplate, double temperature, int max_tokens)
    {
        return kernel.CreateFunctionFromPrompt(
            promptTemplate,
            new PromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "temperature", temperature },
                    { "max_tokens", max_tokens }
                }
            }
        );
    }
}