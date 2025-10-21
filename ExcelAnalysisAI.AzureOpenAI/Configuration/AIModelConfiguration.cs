using ExcelAnalysisAI.AzureOpenAI.Models;

namespace ExcelAnalysisAI.AzureOpenAI.Configuration;

public class AIModelConfiguration
{
    public required OpenAIModelType Type { get; init; }
    public required string Endpoint { get; init; }
    public required string ApiKey { get; init; }
    public required string DeploymentName { get; init; }
}