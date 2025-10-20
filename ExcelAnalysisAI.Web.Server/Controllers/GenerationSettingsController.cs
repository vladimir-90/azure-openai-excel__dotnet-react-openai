using ExcelAnalysisAI.AzureOpenAI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalysisAI.Web.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenerationSettingsController : ControllerBase
{
    [HttpGet("available-ai-models")]
    public IActionResult GetAvailableModels()
    {
        var options = (OpenAIModelType[])Enum.GetValues(typeof(OpenAIModelType));
        return Ok(options!.Select(x => x.ToString()).ToArray());
    }
}