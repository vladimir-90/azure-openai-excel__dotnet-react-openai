using AzureExcelChat.Utility;
using Microsoft.Extensions.Configuration;

// 🚀 Azure Excel Chat - Chat with your Excel files using Azure OpenAI!

Console.WriteLine("📊💬 Azure Excel Chat");
Console.WriteLine("════════════════════════════════════════");
Console.WriteLine("Chat with your Excel files using natural language powered by Azure OpenAI!");
Console.WriteLine();

// Configuration setup for API keys and settings
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

string excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data\\employees-10.xlsx");
Console.WriteLine($"📁  Using default Excel file path: {excelFilePath}");

Console.WriteLine();
Console.WriteLine("📋 Excel Schema:");
string schema = await File.ReadAllTextAsync(
    Path.Combine(Directory.GetCurrentDirectory(), "data\\employees-10__schema.txt")
);
Console.WriteLine(schema);

string actualWorksheetName = "Employees";

Console.WriteLine();
Console.WriteLine("💬 Chat with your Excel file! Type 'exit' to quit.");
Console.WriteLine("════════════════════════════════════════");

var kernel = KernelConstruction.Create(
    config["AZURE_OPENAI_ENDPOINT"]!,
    config["AZURE_OPENAI_API_KEY"]!,
    config["AZURE_OPENAI_DEPLOYMENT_NAME"]!
);
var fn_getQueryDescription = KernelConstruction.CreateFunction(kernel, Prompts.QueryDescription, 0.1, 200);
var fn_getAnswer = KernelConstruction.CreateFunction(kernel, Prompts.FinalAnswer, 0.3, 300);

while (true)
{
    Console.Write("🤖 > ");
    string userInput = Console.ReadLine() ?? "";
    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("👋 Goodbye! Thanks for using Azure Excel Chat!");
        break;
    }

    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }

    try
    {
        Console.WriteLine();

        // Step 1: Analyze the query using AI
        var queryResult = await kernel.InvokeAsync(
            fn_getQueryDescription,
            new() { ["input"] = userInput, ["schema"] = schema }
        );
        string queryDescription = queryResult.GetValue<string>()!.Trim();
        Console.WriteLine($"🔍 Query Analysis: {queryDescription}");

        // Step 2: Execute the query on Excel data
        var dataAll = ExcelUtility.ReadExcelWorksheet(excelFilePath, actualWorksheetName);
        var dataFiltered = RagFiltration.FilterDataBasedOnQuery(dataAll, userInput, queryDescription);
        string excel_data_str = string.Join('\n', dataFiltered.Select(data_line => string.Join("\t", data_line)));
        if (string.IsNullOrWhiteSpace(excel_data_str))
        {
            Console.WriteLine("❌ No data found for that query.");
            Console.WriteLine();
            continue;
        }
        else
        {
            Console.WriteLine($"📊 Excel Data Retrieved:");
            Console.WriteLine($"```");
            Console.WriteLine(excel_data_str);
            Console.WriteLine($"```");
        }

        // Step 3: Generate natural language answer
        var finalAnswerResult = await kernel.InvokeAsync(
            fn_getAnswer,
            new() { ["input"] = userInput, ["data"] = excel_data_str }
        );

        Console.WriteLine($"💡 Answer: {finalAnswerResult.GetValue<string>()}");
        Console.WriteLine();
        Console.WriteLine("════════════════════════════════════════");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
        Console.WriteLine();
    }
}