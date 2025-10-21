using AzureExcelChat.Console.Utility;
using ExcelAnalysisAI.AzureOpenAI.Configuration;
using ExcelAnalysisAI.AzureOpenAI.Models;
using ExcelAnalysisAI.Core.Utility;
using ExcelAnalysisAI.Processing.Core;
using ExcelAnalysisAI.Processing.Core.Contracts;
using ExcelAnalysisAI.Processing.InitialSample.Constants;
using ExcelAnalysisAI.Processing.InitialSample.Extensions;
using ExcelAnalysisAI.Processing.InitialSample.Handling;
using Microsoft.SemanticKernel;

namespace ExcelAnalysisAI.Processing.InitialSample;

public class AIExcelQueryProcessor_InitialSample : IAIExcelQueryProcessor
{
    private readonly Kernel _kernel;
    private readonly OpenAIModelType _openAIModelType;
    private readonly KernelFunction fn_getQueryDescription;
    private readonly KernelFunction fn_getAnswer;
    public AIExcelQueryProcessor_InitialSample(AIModelConfiguration config)
    {
        _kernel = KernelConstruction.Create(config.Endpoint, config.ApiKey, config.DeploymentName);
        _openAIModelType = config.Type;
        fn_getQueryDescription = KernelConstruction.CreateFunction(_kernel, Prompts.QueryDescription, 0.1, 200);
        fn_getAnswer = KernelConstruction.CreateFunction(_kernel, Prompts.FinalAnswer, 0.3, 300);
    }

    public async Task<AIQueryResult> Execute(string userQuery, ExcelFileInfo excelFileInfo)
    {
        var results = new AIQueryResult
        {
            UserQuery = userQuery
        };

        // Analyze the query using AI

        var fnResult_getQueryDescription = await _kernel.InvokeAsync(
            fn_getQueryDescription,
            new() { ["input"] = userQuery, ["schema"] = excelFileInfo.Schema }
        );

        var fnInfo_getDescription = fnResult_getQueryDescription.ToInfo(_openAIModelType);
        results.Requests.Add(fnInfo_getDescription);

        string userQueryDescription = fnInfo_getDescription.Response;

        // Excel data retrieval and filtration based on query dedscription analysis

        var dataAll = ExcelUtility.ReadWorksheet(excelFileInfo.FilePath, excelFileInfo.WorksheetName);
        var dataFiltered = PreFiltrationService.FilterDataBasedOnQuery(dataAll!, userQuery, userQueryDescription);
        string excel_data_str = string.Join('\n', dataFiltered.Select(ln => string.Join('\t', ln)));

        // Generate natural language answer

        if (!dataFiltered.Any())
        {
            results.Requests.Add(new AIRequestResponseInfo
            {
                Request = null,
                Response = "❌ No data found for the query. AI 'final' answer was not requested",
                Cost = null,
                IsSynthetic = true
            });
        }
        else
        {
            var fnResult_getAnswer = await _kernel.InvokeAsync(
                fn_getAnswer,
                new() { ["input"] = userQuery, ["data"] = excel_data_str }
            );

            var fnInfo_finalAnswer = fnResult_getAnswer.ToInfo(_openAIModelType);
            results.Requests.Add(fnInfo_finalAnswer);
        }

        // Result

        return results;
    }
}