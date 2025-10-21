using ExcelAnalysisAI.AzureOpenAI.Models;

namespace ExcelAnalysisAI.AzureOpenAI.Pricings;

public class OpenAIModelPricing
{
    public required decimal Input { get; init; }
    public required decimal CachedInput { get; init; }
    public required decimal Output { get; init; }

    // Azure OpenAI pricings (per 1M Tokens, 2025-10-21)
    // https://azure.microsoft.com/en-us/pricing/details/cognitive-services/openai-service/
    private static readonly IReadOnlyDictionary<OpenAIModelType, OpenAIModelPricing> _pricings = new Dictionary<OpenAIModelType, OpenAIModelPricing>
    {
        {
            OpenAIModelType.GPT_41_nano,
            new OpenAIModelPricing{ Input = 0.10m, CachedInput = 0.03m, Output = 0.40m }
        },
        {
            OpenAIModelType.GPT_41_mini,
            new OpenAIModelPricing{ Input = 0.40m, CachedInput = 0.10m, Output = 1.60m}
        },
        {
            OpenAIModelType.GPT_41,
            new OpenAIModelPricing{ Input = 2, CachedInput = 0.50m, Output = 8}
        },
        {
            OpenAIModelType.GPT_5_nano,
            new OpenAIModelPricing{ Input = 0.05m, CachedInput = 0.01m, Output = 0.4m }
        },
        {
            OpenAIModelType.GPT_5_mini,
            new OpenAIModelPricing{ Input = 0.25m, CachedInput = 0.03m, Output = 2 }
        },
        {
            OpenAIModelType.GPT_5_chat,
            new OpenAIModelPricing{ Input = 1.25m, CachedInput = 0.13m, Output = 10 }
        },
        {
            OpenAIModelType.GPT_o3_mini,
            new OpenAIModelPricing{ Input = 1.10m, CachedInput = 0.55m, Output = 4.40m}
        },
        {
            OpenAIModelType.GPT_o4_mini,
            new OpenAIModelPricing{ Input = 1.10m, CachedInput = 0.28m, Output = 4.40m}
        },
    };

    public static OpenAIModelPricing ForModel(OpenAIModelType modelType) => _pricings[modelType];
}