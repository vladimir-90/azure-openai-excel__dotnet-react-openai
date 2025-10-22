using ExcelAnalysisAI.AzureOpenAI.Configuration;
using ExcelAnalysisAI.AzureOpenAI.Models;

namespace ExcelAnalysisAI.Web.Server.Infrastructure;

public class AzureOpenAIConfig
{
    public required string Endpoint { get; set; }
    public required string ApiKey { get; set; }
    public List<AIModelConfig> Models { get; set; } = new();

    public AIModelConfiguration GetModelConfig(OpenAIModelType modelType)
    {
        var modelInfo = Models.First(x => x.ModelType == modelType);
        return new AIModelConfiguration
        {
            Type = modelInfo.ModelType,
            Endpoint = Endpoint,
            ApiKey = ApiKey,
            DeploymentName = modelInfo.DeploymentName
        };
    }
}

public class AIModelConfig
{
    public required string DeploymentName { get; set; }
    public required OpenAIModelType ModelType { get; set; }
}