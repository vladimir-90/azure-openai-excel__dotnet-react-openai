using ExcelAnalysisAI.AzureOpenAI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalysisAI.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenerationSettingsController(IWebHostEnvironment _env) : ControllerBase
{
    [HttpGet("available-ai-models")]
    public IActionResult GetAvailableModels()
    {
        var modelTypes = (OpenAIModelType[])Enum.GetValues(typeof(OpenAIModelType));
        var options = modelTypes!.Select(x => x.ToString()).ToArray();
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
}