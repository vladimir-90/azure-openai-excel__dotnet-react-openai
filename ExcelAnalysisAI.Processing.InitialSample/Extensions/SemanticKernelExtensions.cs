using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.AzureOpenAI.SemanticKernel.Costs;
using ExcelAnalysisAI.Processing.Core.Contracts;
using Microsoft.SemanticKernel;

namespace ExcelAnalysisAI.Processing.InitialSample.Extensions;

internal static class SemanticKernelExtensions
{
    public static AIRequestResponseInfo ToInfo(this FunctionResult fnResult, OpenAIModelType aiModelType)
        => new()
        {
            Request = fnResult.RenderedPrompt!,
            Response = fnResult.GetValue<string>()!,
            Cost = OpenAIModelCostsCalculator.CalculateDetailedCost(fnResult, aiModelType)!
        };
}