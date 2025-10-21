namespace ExcelAnalysisAI.AzureOpenAI.Costs;

public class QueryDetailedCost
{
    public int InputTokenCount { get; set; }
    public int OutputTokenCount { get; set; }
    public decimal TotalCost { get; set; }
}