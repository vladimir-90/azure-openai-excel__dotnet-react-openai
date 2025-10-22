using ExcelAnalysisAI.AzureOpenAI.Models;

namespace ExcelAnalysisAI.Web.Server.Infrastructure;

public class AzureOpenAIConfig
{
    public required string Endpoint { get; set; }
    public required string ApiKey { get; set; }
    public List<AIModelConfig> Models { get; set; } = new();
}

public class AIModelConfig
{
    public required string DeploymentName { get; set; }
    public required OpenAIModelType ModelType { get; set; }
}