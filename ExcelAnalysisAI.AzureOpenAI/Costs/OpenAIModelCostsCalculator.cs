using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.AzureOpenAI.Pricings;
using Microsoft.SemanticKernel;
using OpenAI.Chat;

namespace ExcelAnalysisAI.AzureOpenAI.Costs;

public static class OpenAIModelCostsCalculator
{
    public static decimal CalculateCost(FunctionResult requestResult, OpenAIModelType modelType)
    {
        var modelPricing = OpenAIModelPricing.ForModel(modelType);

        if (requestResult.Metadata!.TryGetValue("Usage", out var usageObj) && usageObj is ChatTokenUsage usage)
        {
            decimal cost = usage.InputTokenCount * modelPricing.Input / 1000000
                + usage.OutputTokenCount * modelPricing.Output / 1000000;
            return cost;
        }

        return -1;
    }

    public static QueryDetailedCost? CalculateDetailedCost(FunctionResult requestResult, OpenAIModelType modelType)
    {
        var modelPricing = OpenAIModelPricing.ForModel(modelType);

        if (requestResult.Metadata!.TryGetValue("Usage", out var usageObj) && usageObj is ChatTokenUsage usage)
        {
            decimal cost = usage.InputTokenCount * modelPricing.Input / 1000000
                + usage.OutputTokenCount * modelPricing.Output / 1000000;

            return new QueryDetailedCost
            {
                InputTokenCount = usage.InputTokenCount,
                OutputTokenCount = usage.OutputTokenCount,
                TotalCost = cost
            };
        }

        return null;
    }

}