using ExcelAnalysisAI.AzureOpenAI.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Chat;

namespace ExcelAnalysisAI.AzureOpenAI.SemanticKernel.KernelWrapper;

public abstract class KernelWrapperBuilder
{
    protected Kernel _kernel = null!;
    protected Dictionary<string, KernelFunction> _functions = new();

    public static KernelWrapperBuilder ForConfiguration(AIModelConfiguration config)
    {
        var kernel = Kernel
            .CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: config.DeploymentName,
                endpoint: config.Endpoint,
                apiKey: config.ApiKey
            )
            .Build();

        if (config.Type == Models.OpenAIModelType.GPT_5_nano
            || config.Type == Models.OpenAIModelType.GPT_5_mini
            || config.Type == Models.OpenAIModelType.GPT_5_chat)
        {
            return new KernelWrapperBuilder_Gpt5(kernel);
        }
        if (config.Type == Models.OpenAIModelType.GPT_o3_mini
            || config.Type == Models.OpenAIModelType.GPT_o4_mini)
        {
            return new KernelWrapperBuilder_O(kernel);
        }
        else
        {
            return new KernelWrapperBuilder_Gpt4(kernel);
        }
    }

    public abstract KernelWrapperBuilder WithFunction(
        string function_local_id, string promptTemplate, int maxTokenCount);

    public KernelWrapper Build() => new KernelWrapper(_kernel, _functions);
}

internal class KernelWrapperBuilder_Gpt4 : KernelWrapperBuilder
{
    public KernelWrapperBuilder_Gpt4(Kernel kernel) => _kernel = kernel;

    public override KernelWrapperBuilder_Gpt4 WithFunction(
        string function_local_id, string promptTemplate, int maxTokenCount)
    {
        _functions[function_local_id] = _kernel.CreateFunctionFromPrompt(
            promptTemplate,
            new PromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "temperature", 0.5 },
                    { "max_tokens", maxTokenCount }
                }
            }
        );
        return this;
    }
}

internal class KernelWrapperBuilder_Gpt5 : KernelWrapperBuilder
{
    public KernelWrapperBuilder_Gpt5(Kernel kernel) => _kernel = kernel;

    public override KernelWrapperBuilder_Gpt5 WithFunction(
        string function_local_id, string promptTemplate, int maxTokenCount)
    {
        _functions[function_local_id] = _kernel.CreateFunctionFromPrompt(
            promptTemplate,
            new OpenAIPromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "max_completion_tokens", maxTokenCount }
                },
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                ReasoningEffort = new ChatReasoningEffortLevel("high")
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            }
        );
        return this;
    }
}

internal class KernelWrapperBuilder_O : KernelWrapperBuilder
{
    public KernelWrapperBuilder_O(Kernel kernel) => _kernel = kernel;

    public override KernelWrapperBuilder_O WithFunction(
        string function_local_id, string promptTemplate, int maxTokenCount)
    {
        _functions[function_local_id] = _kernel.CreateFunctionFromPrompt(
            promptTemplate,
            new OpenAIPromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "max_completion_tokens", maxTokenCount }
                },
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                ReasoningEffort = new ChatReasoningEffortLevel("medium")
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            }
        );
        return this;
    }
}

public class KernelWrapper
{
    private readonly Kernel _kernel;
    private readonly Dictionary<string, KernelFunction> _functions;
    public KernelWrapper(Kernel kernel, Dictionary<string, KernelFunction> functions)
    {
        _kernel = kernel;
        _functions = functions;
    }

    public Task<FunctionResult> InvokeFunction(string function_local_key, KernelArguments args)
        => _kernel.InvokeAsync(_functions[function_local_key], args);
}