using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.AzureOpenAI.Pricing;
using ExcelAnalysisAI.AzureOpenAI.SemanticKernel.Helpers;
using ExcelAnalysisAI.Core.Extensions;
using ExcelAnalysisAI.Processing.Core.Contracts;
using ExcelAnalysisAI.Processing.InitialSample;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var queryProcessingSvc = new AIExcelQueryProcessor_InitialSample(new()
{
    Type = OpenAIModelType.GPT_41_nano,
    Endpoint = configuration["AZURE_OPENAI_ENDPOINT"]!,
    ApiKey = configuration["AZURE_OPENAI_API_KEY"]!,
    DeploymentName = configuration["AZURE_OPENAI_DEPLOYMENT_NAME"]!
}, CustomReasoningLevel.Low);

var excelFileInfo = new ExcelFileInfo
{
    FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data\\employees-10.xlsx"),
    Schema = await File.ReadAllTextAsync(
        Path.Combine(Directory.GetCurrentDirectory(), "Data\\employees-10__schema.txt")
    ),
    WorksheetName = "Employees"
};

while (true)
{
    Console.Write("\n>>>>>> Your question: ");
    string userInput = Console.ReadLine() ?? "";
    if (!userInput.Valuable())
        continue;
    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var result = await queryProcessingSvc.Execute(userInput, excelFileInfo);

    foreach (var reqRes in result.Requests)
    {
        if (reqRes.IsSynthetic)
        {
            Console.WriteLine(
@$"
        [System]:

{reqRes.Response}");
        }
        else
        {
            var cost = reqRes.Cost!;
            Console.WriteLine(
$@"      
        [System]:

{reqRes.Request}

        [AI]:

{reqRes.Response}

>>>>>> Cost:  input {cost.InputTokenCount} tokens / output: {cost.OutputTokenCount} tokens / {cost.TotalCost} $");
        }
    }

    var summ = new OpenAIQueryCost
    {
        InputTokenCount = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Cost!.InputTokenCount),
        OutputTokenCount = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Cost!.OutputTokenCount),
        ReasoningTokenCount = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Cost!.ReasoningTokenCount),
        TotalCost = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Cost!.TotalCost)
    };
    Console.WriteLine(
@$"
        [TOTAL COST]:

- input {summ.InputTokenCount} tokens
- output: {summ.OutputTokenCount} tokens
- {summ.TotalCost} $");

    Console.WriteLine("\n════════════════════════════════════════");
}