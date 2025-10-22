using ExcelAnalysisAI.AzureOpenAI.Configuration;
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

public static class AzureOpenAIConfigExtensions
{
    public static AIModelConfiguration GetModelConfig(this AzureOpenAIConfig openAI,
        OpenAIModelType modelType)
    {
        var model = openAI.Models.First(x => x.ModelType == modelType);
        return new AIModelConfiguration
        {
            Type = model.ModelType,
            Endpoint = openAI.Endpoint,
            ApiKey = openAI.ApiKey,
            DeploymentName = model.DeploymentName
        };
    }

    public static AIModelConfiguration GetModelConfig(this List<AzureOpenAIConfig> openAIConfigs,
        OpenAIModelType modelType)
    {
        foreach (var openAI in openAIConfigs)
        {
            var model = openAI.GetModelConfig(modelType);
            if (model != null)
                return model;
        }

        throw new NotSupportedException($"'{modelType}' configuration is absent");
    }
}