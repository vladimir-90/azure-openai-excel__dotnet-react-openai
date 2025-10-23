using OpenAI.Chat;

namespace ExcelAnalysisAI.AzureOpenAI.SemanticKernel.Helpers;

/// <summary>
/// Custom "thinking mode level" abstraction
/// </summary>
public enum CustomReasoningLevel
{
    Low,
    Medium,
    High
}

public static class CustomReasoningLevelExtensions
{
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static ChatReasoningEffortLevel ToKernelEntity(this CustomReasoningLevel reasoningLevel)
        => new ChatReasoningEffortLevel(reasoningLevel.ToString().ToLower());
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    // these values are just our "mostly liked" values for different AI model thinking modes
    public static int GetMaxTokenCount(this CustomReasoningLevel level) => level switch
    {
        CustomReasoningLevel.Low => 200,
        CustomReasoningLevel.Medium => 1000,
        CustomReasoningLevel.High => 5000,
        _ => throw new ArgumentOutOfRangeException()
    };
}