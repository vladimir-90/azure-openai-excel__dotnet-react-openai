using AzureExcelChat.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

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

var kernel = KernelConstruction.Create(
    config["AZURE_OPENAI_ENDPOINT"]!,
    config["AZURE_OPENAI_API_KEY"]!,
    config["AZURE_OPENAI_DEPLOYMENT_NAME"]!
);

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

var textToQueryFunction = CreateTextToQueryFunction(kernel, schema);
var finalAnswerFunction = CreateFinalAnswerFunction(kernel);

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
        var queryResult = await textToQueryFunction.InvokeAsync(kernel, new() { ["input"] = userInput });
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
        var finalAnswerResult = await finalAnswerFunction.InvokeAsync(kernel, new()
        {
            ["input"] = userInput,
            ["data"] = excel_data_str
        });

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

KernelFunction CreateTextToQueryFunction(Kernel kernel, string schema)
{
    const string prompt = @"
Given the following Excel worksheet data structure, analyze the user's question and describe what data should be filtered or retrieved.
- Describe the filtering logic clearly
- Mention specific column names and conditions
- Be specific about what data should be returned
- If it's a calculation (like average, sum, count), mention that clearly
- Focus on the most relevant data for the user's question

Schema:
---
{{$schema}}
---

User Question: {{$input}}

Query Description (be specific and actionable):
";

    var executionSettings = new PromptExecutionSettings()
    {
        ExtensionData = new Dictionary<string, object>()
        {
            { "temperature", 0.1 },
            { "max_tokens", 200 }
        }
    };

    var promptConfig = new PromptTemplateConfig
    {
        Template = prompt.Replace("{{$schema}}", schema),
        ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
        {
            { "default", executionSettings }
        }
    };

    return KernelFunctionFactory.CreateFromPrompt(promptConfig);
}

KernelFunction CreateFinalAnswerFunction(Kernel kernel)
{
    const string prompt = @"
Answer the following user's question based ONLY on the provided data from the Excel file.
- Be friendly, conversational, and concise
- If calculations are needed, perform them accurately
- If the data is empty or insufficient, say you could not find an answer
- Use specific numbers and names when available
- Format numbers appropriately (e.g., $95,000 for salaries)
- For department listing questions, extract and list the unique departments from the data
- For department counts, count unique occurrences

Data from Excel:
---
{{$data}}
---

User Question: {{$input}}

Answer:
";

    var executionSettings = new PromptExecutionSettings()
    {
        ExtensionData = new Dictionary<string, object>()
        {
            { "temperature", 0.3 },
            { "max_tokens", 300 }
        }
    };

    var promptConfig = new PromptTemplateConfig
    {
        Template = prompt,
        ExecutionSettings = new Dictionary<string, PromptExecutionSettings>
        {
            { "default", executionSettings }
        }
    };

    return KernelFunctionFactory.CreateFromPrompt(promptConfig);
}