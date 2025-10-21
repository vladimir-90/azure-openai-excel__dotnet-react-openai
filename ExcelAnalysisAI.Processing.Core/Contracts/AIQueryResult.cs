using ExcelAnalysisAI.AzureOpenAI.Pricing;

namespace ExcelAnalysisAI.Processing.Core.Contracts;

public class AIQueryResult
{
    public required string UserQuery { get; set; }
    public List<AIRequestResponseInfo> Requests { get; set; } = new List<AIRequestResponseInfo>();
}

public class AIRequestResponseInfo
{
    public required string? Request { get; init; }
    public required string Response { get; init; }
    public required OpenAIQueryCost? Cost { get; init; }
    public bool IsSynthetic { get; init; } = false;     // no AI call, just possibility to send "system" messages
}