using ClosedXML.Excel;
using ExcelAnalysisAI.TestData.Console.Employees;

namespace ExcelAnalysisAI.TestData.Console.Utility;

internal class ExcelPersistenceUtility
{
    public void SaveToFile(List<Employee> employees, string filePath)
    {
        // Delete existing file if it exists
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Employees");

        // Header row
        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Region";
        worksheet.Cell(1, 4).Value = "Department";
        worksheet.Cell(1, 5).Value = "Salary";
        worksheet.Cell(1, 6).Value = "HireDate";
        worksheet.Cell(1, 7).Value = "YearsExperience";
        worksheet.Cell(1, 8).Value = "HasHigherEducation";

        // Style header row: bold and centered
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Data rows
        for (int i = 0; i < employees.Count; i++)
        {
            var employee = employees[i];
            int rowIndex = i + 2; // Start from row 2

            worksheet.Cell(rowIndex, 1).Value = employee.Id;
            worksheet.Cell(rowIndex, 2).Value = employee.Name;
            worksheet.Cell(rowIndex, 3).Value = employee.Region;
            worksheet.Cell(rowIndex, 4).Value = employee.Department;
            worksheet.Cell(rowIndex, 5).Value = employee.Salary;
            worksheet.Cell(rowIndex, 6).Value = employee.HireDate;
            worksheet.Cell(rowIndex, 7).Value = employee.YearsExperience;
            worksheet.Cell(rowIndex, 8).Value = employee.HasHigherEducation;
        }

        // Style data rows: left-aligned and not bold
        if (employees.Count > 0)
        {
            var dataRange = worksheet.Range(2, 1, employees.Count + 1, 8);
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dataRange.Style.Font.Bold = false;
        }

        // Auto-fit columns for better readability
        worksheet.Columns().AdjustToContents();

        workbook.SaveAs(filePath);
    }
}