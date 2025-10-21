namespace ExcelAnalysisAI.AzureOpenAI.Pricing;

public class OpenAIQueryCost
{
    public int InputTokenCount { get; set; }
    public int OutputTokenCount { get; set; }
    public decimal TotalCost { get; set; }
}