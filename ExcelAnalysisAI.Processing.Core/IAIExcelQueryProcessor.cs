using ExcelAnalysisAI.Processing.Core.Contracts;

namespace ExcelAnalysisAI.Processing.Core;

public interface IAIExcelQueryProcessor
{
    Task<AIQueryResult> Execute(string userQuery, ExcelFileInfo excelFileInfo);
}