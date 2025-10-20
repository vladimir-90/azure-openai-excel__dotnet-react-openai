using Microsoft.SemanticKernel;
using OpenAI.Chat;

namespace AzureExcelChat.Utility;

//      Pricing is here:
// https://azure.microsoft.com/en-us/pricing/details/cognitive-services/openai-service/

internal static class OpenAIModelCostsCalculator
{
    // Azure OpenAI pricings (per 1M Tokens, 2025-10-14)
    private static readonly Dictionary<OpenAIModelType, OpenAIModelPricing> _pricings = new()
    {
        {
            OpenAIModelType.Gtp5Nano_Global,
            new OpenAIModelPricing{ Input = 0.05m, CachedInput = 0.01m, Output = 0.4m }
        },
        {
            OpenAIModelType.Gpt41Nano_Global,
            new OpenAIModelPricing{ Input = 0.10m, CachedInput = 0.03m, Output = 0.40m }
        }
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

enum OpenAIModelType
{
    Gtp5Nano_Global,
    Gpt41Nano_Global
}

internal class QueryDetailedCost
{
    public int InputTokenCount { get; set; }
    public int OutputTokenCount { get; set; }
    public decimal TotalCost { get; set; }
}