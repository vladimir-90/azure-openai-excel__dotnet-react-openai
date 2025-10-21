using ExcelAnalysisAI.AzureOpenAI.Models;
using Microsoft.SemanticKernel;
using OpenAI.Chat;

namespace ExcelAnalysisAI.AzureOpenAI.Costs;

//      Pricing is taken from here on 2025-10-21:
// https://azure.microsoft.com/en-us/pricing/details/cognitive-services/openai-service/

public static class OpenAIModelCostsCalculator
{
    // Azure OpenAI pricings (per 1M Tokens, 2025-10-14)
    private static readonly Dictionary<OpenAIModelType, OpenAIModelPricing> _pricings = new()
    {
        {
            OpenAIModelType.GPT_41_nano,
            new OpenAIModelPricing{ Input = 0.10m, CachedInput = 0.03m, Output = 0.40m }
        },
        {
            OpenAIModelType.GPT_41_mini,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output =  1.60m}
        },
        {
            OpenAIModelType.GPT_41,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output =  8}
        },
        {
            OpenAIModelType.GPT_5_nano,
            new OpenAIModelPricing{ Input = 0.05m, CachedInput = 0.01m, Output = 0.4m }
        },
        {
            OpenAIModelType.GPT_5_mini,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output = 2 }
        },
        {
            OpenAIModelType.GPT_5_chat,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output = 10 }
        },
        {
            OpenAIModelType.GPT_o3_mini,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output = 1.60m}
        },
        {
            OpenAIModelType.GPT_o4_mini,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output = 1.60m}
        },
    };

    public static decimal CalculateCost(FunctionResult requestResult, OpenAIModelType modelType)
    {
        var modelPricing = _pricings[modelType];

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
        var modelPricing = _pricings[modelType];

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

    private class OpenAIModelPricing
    {
        public required decimal Input { get; init; }
        public required decimal CachedInput { get; init; }
        public required decimal Output { get; init; }
    }
}