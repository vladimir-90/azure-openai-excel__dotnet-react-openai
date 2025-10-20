using AzureExcelChat.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace AzureExcelChat.InitialSample;

internal class QueryProcessingService
{
    private readonly Kernel _kernel;
    private readonly KernelFunction fn_getQueryDescription;
    private readonly KernelFunction fn_getAnswer;
    private readonly OpenAIModelType _openAIModelType;
    public QueryProcessingService(IConfiguration configuration, OpenAIModelType openAIModelType)
    {
        _kernel = KernelConstruction.Create(
            configuration["AZURE_OPENAI_ENDPOINT"]!,
            configuration["AZURE_OPENAI_API_KEY"]!,
            configuration["AZURE_OPENAI_DEPLOYMENT_NAME"]!
        );
        _openAIModelType = openAIModelType;
        fn_getQueryDescription = KernelConstruction.CreateFunction(_kernel, Prompts.QueryDescription, 0.1, 200);
        fn_getAnswer = KernelConstruction.CreateFunction(_kernel, Prompts.FinalAnswer, 0.3, 300);
    }

    public async Task<QueryProcessingResult> Execute(string userQuery, ExcelFileInfo excelFileInfo)
    {
        var results = new QueryProcessingResult
        {
            UserQuery = userQuery
        };

        // Analyze the query using AI

        var fnResult_getQueryDescription = await _kernel.InvokeAsync(
            fn_getQueryDescription,
            new() { ["input"] = userQuery, ["schema"] = excelFileInfo.Schema }
        );

        var fnInfo_getDescription = GetRequestInfo(fnResult_getQueryDescription);
        results.Requests.Add(fnInfo_getDescription);

        string userQueryDescription = fnInfo_getDescription.Response;

        // Excel data retrieval and filtration based on query dedscription analysis

        var dataAll = ExcelUtility.ReadExcelWorksheet(excelFileInfo.FilePath, excelFileInfo.WorksheetName);     // to_do: ReadWorksheet
        var dataFiltered = RagFiltration.FilterDataBasedOnQuery(dataAll!, userQuery, userQueryDescription);     // to_do: PreFiltrationService
        string excel_data_str = string.Join('\n', dataFiltered.Select(ln => string.Join('\t', ln)));

        // Generate natural language answer

        if (!dataFiltered.Any())
        {
            results.Requests.Add(new QueryProcessingResult.AIRequestResponseInfo
            {
                Request = null,
                Response = "❌ No data found for the query. AI 'final' answer was not requested",
                Costs = null,
                IsSynthetic = true
            });
        }
        else
        {
            var fnResult_getAnswer = await _kernel.InvokeAsync(
                fn_getAnswer,
                new() { ["input"] = userQuery, ["data"] = excel_data_str }
            );

            var fnInfo_finalAnswer = GetRequestInfo(fnResult_getAnswer);
            results.Requests.Add(fnInfo_finalAnswer);
        }

        // Result

        return results;
    }

    private QueryProcessingResult.AIRequestResponseInfo GetRequestInfo(FunctionResult fnResult) => new()
    {
        Request = fnResult.RenderedPrompt!,
        Response = fnResult.GetValue<string>()!,
        Costs = OpenAIModelCostsCalculator.CalculateDetailedCost(fnResult, _openAIModelType)!
    };
}

internal class QueryProcessingResult
{
    public required string UserQuery { get; set; }
    public List<AIRequestResponseInfo> Requests { get; set; } = new List<AIRequestResponseInfo>();

    internal class AIRequestResponseInfo
    {
        public required string? Request { get; init; }
        public required string Response { get; init; }
        public required QueryDetailedCost? Costs { get; init; }
        public bool IsSynthetic { get; init; } = false;
    }
}

internal class ExcelFileInfo
{
    public required string FilePath { get; init; }
    public required string WorksheetName { get; init; }
    public required string Schema { get; init; }
}