using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.Processing.Core.Contracts;
using ExcelAnalysisAI.Processing.InitialSample;
using ExcelAnalysisAI.Web.Server.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalysisAI.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExcelAnalysisController(IWebHostEnvironment _env, AzureOpenAIConfig _openAIConfig) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ExecuteExcelQuery([FromBody] ExcelAnalysisQueryDto dto)
    {
        var aiModelInfo = _openAIConfig.Models.First(x => x.ModelType == dto.ModelType);
        var queryProcessor = new AIExcelQueryProcessor_InitialSample(new()
        {
            Type = aiModelInfo.ModelType,
            Endpoint = _openAIConfig.Endpoint,
            ApiKey = _openAIConfig.ApiKey,
            DeploymentName = aiModelInfo.DeploymentName
        });

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
    public required string DatasetName { get; set; }
    public required string Question { get; set; }
}