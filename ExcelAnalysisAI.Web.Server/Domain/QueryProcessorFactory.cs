using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.AzureOpenAI.SemanticKernel.Helpers;
using ExcelAnalysisAI.Processing.Core;
using ExcelAnalysisAI.Processing.InitialSample;
using ExcelAnalysisAI.Web.Server.Infrastructure;

namespace ExcelAnalysisAI.Web.Server.Domain;

public interface IQueryProcessorFactory
{
    IAIExcelQueryProcessor Create(OpenAIModelType modelType, CustomReasoningLevel customReasoning);
}

public class QueryProcessorFactory(List<AzureOpenAIConfig> _openAIConfigs) : IQueryProcessorFactory
{
    public IAIExcelQueryProcessor Create(OpenAIModelType modelType, CustomReasoningLevel customReasoning)
    {
        var aiModelConfig = _openAIConfigs.GetModelConfig(modelType);
        if (aiModelConfig is null)
            throw new NotSupportedException($"'{modelType}' configuration is absent");

        var queryProcessor = new AIExcelQueryProcessor_InitialSample(aiModelConfig, customReasoning);

        return queryProcessor;
    }
}