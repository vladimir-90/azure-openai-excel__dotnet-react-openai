using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.AzureOpenAI.Pricings;
using ExcelAnalysisAI.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ExcelAnalysisAI.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenerationSettingsController(IWebHostEnvironment _env) : ControllerBase
{
    [HttpGet("available-ai-models")]
    public IActionResult GetAvailableModels()
    {
        var modelTypes = (OpenAIModelType[])Enum.GetValues(typeof(OpenAIModelType));
        var options = modelTypes!
            .Select(x => new AIModelDto
            {
                ModelType = x.ToString(),
                Label = x.GetType().GetField(x.ToString())!.GetCustomAttribute<DisplayAttribute>()!.Name!,
                Pricing = OpenAIModelPricing.ForModel(x)
            })
            .ToArray();
        return Ok(options);
    }

    [HttpGet("test-data-sets")]
    public IActionResult GetTestDataSets()
    {
        var rootDataDir = Path.Combine(_env.ContentRootPath, "Data");
        var dataDirs = Directory.GetDirectories(rootDataDir)!;
        var options = dataDirs.Select(x => new DirectoryInfo(x).Name).ToArray();
        return Ok(options);
    }

    [HttpGet("test-data-sets/{datasetName}")]
    public async Task<IActionResult> GetTestDataset([FromRoute] string datasetName)
    {
        var dirPath = Path.Combine(_env.ContentRootPath, "Data", datasetName);
        var schema = await System.IO.File.ReadAllTextAsync(Path.Combine(dirPath, "schema.txt"));
        var data = ExcelUtility.ReadWorksheet(Path.Combine(dirPath, "data.xlsx"), "Employees");

        return Ok(new DataSetInfoDto
        {
            Name = datasetName,
            Schema = schema,
            DataSlice = data.Take(6).ToList(),      // 1st row are headings
            TotalEntityCount = data.Count - 1       // 1st row are headings
        });
    }
}

public class AIModelDto
{
    public required string ModelType { get; set; }
    public required string Label { get; set; }
    public required OpenAIModelPricing Pricing { get; set; }
}

public class DataSetInfoDto
{
    public required string Name { get; set; }
    public required string Schema { get; set; }
    public required List<List<object>> DataSlice { get; set; }
    public required int TotalEntityCount { get; set; }
}