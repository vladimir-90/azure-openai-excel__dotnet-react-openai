namespace ExcelAnalysisAI.Core.Extensions;

public static class StringExtensions
{
    public static bool Valuable(this string? str) => !string.IsNullOrWhiteSpace(str);
}
