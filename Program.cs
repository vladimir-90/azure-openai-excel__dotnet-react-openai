using AzureExcelChat.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System.Text;
using System.Text.RegularExpressions;

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

try
{
    var kernel = BuildKernel(config);

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
            string excelData = await ExecuteExcelQueryAndFormatResults(excelFilePath, actualWorksheetName, userInput, queryDescription);
            if (string.IsNullOrWhiteSpace(excelData))
            {
                Console.WriteLine("❌ No data found for that query.");
                Console.WriteLine();
                continue;
            }
            Console.WriteLine($"📊 Excel Data Retrieved:");
            Console.WriteLine($"```");
            Console.WriteLine(excelData);
            Console.WriteLine($"```");

            // Step 3: Generate natural language answer
            var finalAnswerResult = await finalAnswerFunction.InvokeAsync(kernel, new()
            {
                ["input"] = userInput,
                ["data"] = excelData
            });

            Console.WriteLine($"💡 Answer: {finalAnswerResult.GetValue<string>()}");
            Console.WriteLine();
            Console.WriteLine("════════════════════════════════════════");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine();

            if (ex.Message.Contains("credentials") || ex.Message.Contains("API") || ex.Message.Contains("authorization"))
            {
                Console.WriteLine("⚙️ Configuration Help:");
                Console.WriteLine("Make sure you have set the following user secrets:");
                Console.WriteLine("1. AZURE_OPENAI_API_KEY: Your Azure OpenAI API key");
                Console.WriteLine("2. AZURE_OPENAI_ENDPOINT: Your Azure OpenAI endpoint URL");
                Console.WriteLine("3. AZURE_OPENAI_DEPLOYMENT_NAME: Your GPT model deployment name");
                Console.WriteLine();
                Console.WriteLine("Use these commands to set them:");
                Console.WriteLine("dotnet user-secrets set \"AZURE_OPENAI_API_KEY\" \"your-key\"");
                Console.WriteLine("dotnet user-secrets set \"AZURE_OPENAI_ENDPOINT\" \"https://your-resource.openai.azure.com/\"");
                Console.WriteLine("dotnet user-secrets set \"AZURE_OPENAI_DEPLOYMENT_NAME\" \"your-deployment\"");
                Console.WriteLine();
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"🚨 Startup Error: {ex.Message}");

    if (ex.Message.Contains("AZURE_OPENAI"))
    {
        Console.WriteLine();
        Console.WriteLine("🛠️ Setup Required:");
        Console.WriteLine("Please configure your Azure OpenAI credentials using user secrets.");
        Console.WriteLine("Run the setup commands shown in the README.md file.");
        Console.WriteLine();
        Console.WriteLine("Quick setup:");
        Console.WriteLine("dotnet user-secrets init");
        Console.WriteLine("dotnet user-secrets set \"AZURE_OPENAI_API_KEY\" \"your-key\"");
        Console.WriteLine("dotnet user-secrets set \"AZURE_OPENAI_ENDPOINT\" \"https://your-resource.openai.azure.com/\"");
        Console.WriteLine("dotnet user-secrets set \"AZURE_OPENAI_DEPLOYMENT_NAME\" \"your-deployment\"");
    }
}

#pragma warning disable SKEXP0010

Kernel BuildKernel(IConfiguration config)
{
    var builder = Kernel.CreateBuilder();

    string? apiKey = config["AZURE_OPENAI_API_KEY"];
    string? endpoint = config["AZURE_OPENAI_ENDPOINT"];
    string? deploymentName = config["AZURE_OPENAI_DEPLOYMENT_NAME"];

    if (string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("AZURE_OPENAI_API_KEY is not set. Please set it using 'dotnet user-secrets set AZURE_OPENAI_API_KEY your-key' or as an environment variable.");
    }

    if (string.IsNullOrEmpty(endpoint))
    {
        throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set. Please set it using 'dotnet user-secrets set AZURE_OPENAI_ENDPOINT your-endpoint' or as an environment variable.");
    }

    if (string.IsNullOrEmpty(deploymentName))
    {
        throw new InvalidOperationException("AZURE_OPENAI_DEPLOYMENT_NAME is not set. Please set it using 'dotnet user-secrets set AZURE_OPENAI_DEPLOYMENT_NAME your-deployment' or as an environment variable.");
    }

    builder.AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey
    );

    return builder.Build();
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

async Task<string> ExecuteExcelQueryAndFormatResults(string filePath, string worksheetName, string userQuery, string queryDescription)
{
    return await Task.Run(() =>
    {
        try
        {
            var dataAll = ExcelUtility.ReadExcelWorksheet(filePath, worksheetName);

            // Filter data based on user query and AI description
            var filteredData = FilterDataBasedOnQuery(dataAll, userQuery, queryDescription);

            var resultBuilder = new StringBuilder();

            // Add headers
            if (filteredData.Count > 0)
            {
                resultBuilder.AppendLine(string.Join("\t", filteredData[0]));

                // Add data rows
                for (int i = 1; i < filteredData.Count; i++)
                {
                    resultBuilder.AppendLine(string.Join("\t", filteredData[i]));
                }
            }

            return resultBuilder.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading Excel file: {ex.Message}");
        }
    });
}

List<List<object>> FilterDataBasedOnQuery(List<List<object>> data, string userQuery, string queryDescription)
{
    var result = new List<List<object>>();
    if (data.Count == 0) return result;

    // Always include headers
    result.Add(data[0]);

    var query = userQuery.ToLower();
    var description = queryDescription.ToLower();

    // Special handling for department listing queries
    if ((query.Contains("list") && (query.Contains("department") || query.Contains("departments"))) ||
        (description.Contains("list") && (description.Contains("department") || description.Contains("departments"))) ||
        (query.Contains("what") && (query.Contains("department") || query.Contains("departments"))) ||
        (query.Contains("show") && (query.Contains("department") || query.Contains("departments"))))
    {
        // For department listing, we want to show all employees so the AI can extract unique departments
        for (int i = 1; i < data.Count; i++)
        {
            result.Add(data[i]);
        }
        return result;
    }

    // Enhanced filtering logic based on user query and AI description
    for (int i = 1; i < data.Count; i++)
    {
        var row = data[i];
        bool includeRow = false;

        // Department-based filtering
        if (query.Contains("engineer") || description.Contains("engineering"))
        {
            includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("engineering") == true;
        }
        else if (query.Contains("sales") || description.Contains("sales"))
        {
            includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("sales") == true;
        }
        else if (query.Contains("hr") || query.Contains("human resources") || description.Contains("hr"))
        {
            includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("hr") == true;
        }
        else if (query.Contains("marketing") || description.Contains("marketing"))
        {
            includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("marketing") == true;
        }
        // Salary-based filtering
        else if (query.Contains("salary") && (query.Contains("more than") || query.Contains("greater than") || query.Contains(">") || description.Contains("greater") || description.Contains("above")))
        {
            if (row.Count > 3 && decimal.TryParse(row[3]?.ToString(), out decimal salary))
            {
                // Extract number from query
                var numbers = Regex.Matches(query, @"\d+");
                if (numbers.Count > 0 && decimal.TryParse(numbers[0].Value, out decimal threshold))
                {
                    includeRow = salary > threshold;
                }
                else
                {
                    includeRow = salary > 90000; // Default threshold
                }
            }
        }
        else if (query.Contains("salary") && (query.Contains("less than") || query.Contains("<") || description.Contains("less") || description.Contains("below")))
        {
            if (row.Count > 3 && decimal.TryParse(row[3]?.ToString(), out decimal salary))
            {
                var numbers = Regex.Matches(query, @"\d+");
                if (numbers.Count > 0 && decimal.TryParse(numbers[0].Value, out decimal threshold))
                {
                    includeRow = salary < threshold;
                }
                else
                {
                    includeRow = salary < 80000; // Default threshold
                }
            }
        }
        // Average calculations - include relevant rows
        else if ((query.Contains("average") || description.Contains("average")) && (query.Contains("salary") || description.Contains("salary")))
        {
            if (query.Contains("sales") || description.Contains("sales"))
            {
                includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("sales") == true;
            }
            else if (query.Contains("engineering") || description.Contains("engineering"))
            {
                includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("engineering") == true;
            }
            else if (query.Contains("hr") || description.Contains("hr"))
            {
                includeRow = row.Count > 2 && row[2]?.ToString()?.ToLower().Contains("hr") == true;
            }
            else
            {
                includeRow = true; // Include all for overall average
            }
        }
        // Name-based searches
        else if (query.Contains("who") || query.Contains("list") || description.Contains("names") || description.Contains("employees"))
        {
            includeRow = true; // Show all employees for "who" questions
        }
        // Date-based filtering
        else if (query.Contains("hired") || query.Contains("hire date") || query.Contains("started") || description.Contains("date") || description.Contains("hired"))
        {
            if (query.Contains("2020") || description.Contains("2020"))
            {
                includeRow = row.Count > 4 && row[4]?.ToString()?.Contains("2020") == true;
            }
            else if (query.Contains("2021") || description.Contains("2021"))
            {
                includeRow = row.Count > 4 && row[4]?.ToString()?.Contains("2021") == true;
            }
            else if (query.Contains("2022") || description.Contains("2022"))
            {
                includeRow = row.Count > 4 && row[4]?.ToString()?.Contains("2022") == true;
            }
            else if (query.Contains("2023") || description.Contains("2023"))
            {
                includeRow = row.Count > 4 && row[4]?.ToString()?.Contains("2023") == true;
            }
            else if (query.Contains("2024") || description.Contains("2024"))
            {
                includeRow = row.Count > 4 && row[4]?.ToString()?.Contains("2024") == true;
            }
            else
            {
                includeRow = true; // Show all dates
            }
        }
        // Count queries
        else if (query.Contains("how many") || query.Contains("count") || description.Contains("count"))
        {
            includeRow = true; // Include all for counting
        }
        else
        {
            includeRow = true; // Default: include all rows for general queries
        }

        if (includeRow)
        {
            result.Add(row);
        }
    }

    return result;
}