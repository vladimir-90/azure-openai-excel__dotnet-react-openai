using Microsoft.SemanticKernel;

namespace AzureExcelChat.Utility;

internal static class KernelConstruction
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
}