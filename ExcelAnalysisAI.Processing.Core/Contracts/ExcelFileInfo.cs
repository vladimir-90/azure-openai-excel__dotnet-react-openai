namespace ExcelAnalysisAI.Processing.Core.Contracts;

public class ExcelFileInfo
{
    public required string FilePath { get; init; }
    public required string WorksheetName { get; init; }
    public required string Schema { get; init; }
}