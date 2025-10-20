using ClosedXML.Excel;

namespace AzureExcelChat.Console.Utility;

internal static class ExcelUtility
{
    public static List<List<object>> ReadWorksheet(string filePath, string worksheetName)
    {
        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(worksheetName);
        if (worksheet == null)
            throw new ArgumentException(nameof(worksheetName));

        var usedRange = worksheet.RangeUsed();
        if (usedRange == null)
            throw new NullReferenceException(nameof(worksheetName));

        // Convert Excel data to list format
        var data = new List<List<object>>();
        foreach (var row in usedRange.Rows())
        {
            var rowData = new List<object>();
            foreach (var cell in row.Cells())
            {
                rowData.Add(cell.Value.ToString() ?? "");
            }
            data.Add(rowData);
        }

        return data;
    }
}