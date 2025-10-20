using AzureExcelChat.InitialSample;
using AzureExcelChat.Utility;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

string excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data\\employees-10.xlsx");
string excelSchema = await File.ReadAllTextAsync(
    Path.Combine(Directory.GetCurrentDirectory(), "data\\employees-10__schema.txt")
);
string excelWorksheet = "Employees";

var kernel = KernelConstruction.Create(
    config["AZURE_OPENAI_ENDPOINT"]!,
    config["AZURE_OPENAI_API_KEY"]!,
    config["AZURE_OPENAI_DEPLOYMENT_NAME"]!
);
var fn_getQueryDescription = KernelConstruction.CreateFunction(kernel, Prompts.QueryDescription, 0.1, 200);
var fn_getAnswer = KernelConstruction.CreateFunction(kernel, Prompts.FinalAnswer, 0.3, 300);

Console.WriteLine("Azure Excel Chat");
Console.WriteLine("════════════════════════════════════════");
Console.WriteLine("Chat with your Excel files using natural language powered by Azure OpenAI!");
Console.WriteLine("Type 'exit' to quit.");
Console.WriteLine("════════════════════════════════════════");

while (true)
{
    Console.Write("\n>>>>>> Your question: ");
    string userInput = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(userInput)) continue;
    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

    // Step 1: Analyze the query using AI
    var queryResult = await kernel.InvokeAsync(
        fn_getQueryDescription,
        new() { ["input"] = userInput, ["schema"] = excelSchema }
    );
    string queryDescription = queryResult.GetValue<string>()!.Trim();
    Console.WriteLine($"\n>>>>>> Query Analysis: \n\n{queryDescription}");

    // Step 2: Execute the query on Excel data
    var dataAll = ExcelUtility.ReadExcelWorksheet(excelFilePath, excelWorksheet);
    var dataFiltered = RagFiltration.FilterDataBasedOnQuery(dataAll, userInput, queryDescription);
    string excel_data_str = string.Join('\n', dataFiltered.Select(data_line => string.Join("\t", data_line)));
    Console.WriteLine($"\n>>>>>> Excel Data Retrieved:\n");
    Console.WriteLine(dataFiltered.Any() ? excel_data_str : "No data found for that query.");

    // Step 3: Generate natural language answer
    if (dataFiltered.Any())
    {
    var finalAnswerResult = await kernel.InvokeAsync(
        fn_getAnswer,
        new() { ["input"] = userInput, ["data"] = excel_data_str }
    );
        Console.WriteLine($"\n>>>>>> Answer:\n\n{finalAnswerResult.GetValue<string>()}");
    }

    Console.WriteLine("\n════════════════════════════════════════");
}