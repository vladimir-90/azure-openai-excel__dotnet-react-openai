using AzureExcelChat.InitialSample;
using AzureExcelChat.Utility;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var queryProcessingSvc = new QueryProcessingService(config, OpenAIModelType.Gpt41Nano_Global);

var excelFileInfo = new ExcelFileInfo
{
    FilePath = Path.Combine(Directory.GetCurrentDirectory(), "data\\employees-10.xlsx"),
    Schema = await File.ReadAllTextAsync(
        Path.Combine(Directory.GetCurrentDirectory(), "data\\employees-10__schema.txt")
    ),
    WorksheetName = "Employees"
};

Console.WriteLine("Azure Excel Chat");
Console.WriteLine("════════════════════════════════════════");
Console.WriteLine("Chat with your Excel files using natural language powered by Azure OpenAI!");
Console.WriteLine("Type 'exit' to quit.");
Console.WriteLine("════════════════════════════════════════");

while (true)
{
    Console.Write("\n>>>>>> Your question: ");
    string userInput = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(userInput))
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
            var cost = reqRes.Costs!;
            Console.WriteLine(
$@"      
        [System]:

{reqRes.Request}

        [AI]:

{reqRes.Response}

>>>>>> Cost:  input {cost.InputTokenCount} tokens / output: {cost.OutputTokenCount} tokens / {cost.TotalCost} $");
        }
    }

    var summ = new QueryDetailedCost
    {
        InputTokenCount = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Costs!.InputTokenCount),
        OutputTokenCount = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Costs!.OutputTokenCount),
        TotalCost = result.Requests.Where(r => !r.IsSynthetic).Sum(r => r.Costs!.TotalCost)
    };
    Console.WriteLine(
@$"
        [TOTAL COST]:

- input {summ.InputTokenCount} tokens
- output: {summ.OutputTokenCount} tokens
- {summ.TotalCost} $");

    Console.WriteLine("\n════════════════════════════════════════");
}