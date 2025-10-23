namespace ExcelAnalysisAI.AzureOpenAI.Pricing;

// Reasoning models have reasoning_tokens as part of completion_tokens_details in the model response. These are hidden tokens that aren't returned as part of the message response content
// but are used by the model to help generate a final answer to your request. reasoning_effort can be set to low, medium, or high for all reasoning models except o1-mini.
// GPT-5 reasoning models support a new reasoning_effort setting of minimal. The higher the effort setting, the longer the model will spend processing the request, which will generally result
// in a larger number of reasoning_tokens.
//      https://learn.microsoft.com/en-us/azure/ai-foundry/openai/how-to/reasoning?tabs=gpt-5%2Cpython%2Cpy

public class OpenAIQueryCost
{
    public required int InputTokenCount { get; set; }
    public required int OutputTokenCount { get; set; }
    public required int ReasoningTokenCount { get; set; }
    public required decimal TotalCost { get; set; }
}