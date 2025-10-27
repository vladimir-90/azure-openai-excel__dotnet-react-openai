using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.AzureOpenAI.SemanticKernel.Helpers;
using ExcelAnalysisAI.Processing.Core.Contracts;
using ExcelAnalysisAI.Web.Server.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalysisAI.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExcelAnalysisController(IWebHostEnvironment _env, IQueryProcessorFactory _queryProcessorFactory)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ExecuteExcelQuery([FromBody] ExcelAnalysisQueryDto dto)
    {
        var queryProcessor = _queryProcessorFactory.Create(dto.ModelType, dto.ReasoningLevel);

        var dirPath = Path.Combine(_env.ContentRootPath, "Data", dto.DatasetName);
        var excelFileInfo = new ExcelFileInfo
        {
            Schema = await System.IO.File.ReadAllTextAsync(Path.Combine(dirPath, "schema.txt")),
            FilePath = Path.Combine(dirPath, "data.xlsx"),
            WorksheetName = "Employees"
        };

        var result = await queryProcessor.Execute(dto.Question, excelFileInfo);

        return Ok(result);
    }
}

public class ExcelAnalysisQueryDto
{
    public required OpenAIModelType ModelType { get; set; }
    public required CustomReasoningLevel ReasoningLevel { get; set; }
    public required string DatasetName { get; set; }
    public required string Question { get; set; }
}