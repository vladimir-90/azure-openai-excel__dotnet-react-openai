using System.ComponentModel.DataAnnotations;

namespace ExcelAnalysisAI.AzureOpenAI.Models;

public enum OpenAIModelType
{
    [Display(Name = "gpt-4.1-nano")]
    GPT_41_nano,

    [Display(Name = "gpt-4.1-mini")]
    GPT_41_mini,

    [Display(Name = "gpt-4.1")]
    GPT_41,

    [Display(Name = "gpt-5-nano")]
    GPT_5_nano,

    [Display(Name = "gpt-5-mini")]
    GPT_5_mini,

    [Display(Name = "gpt-5-chat")]
    GPT_5_chat,

    [Display(Name = "o3-mini")]
    GPT_o3_mini,

    [Display(Name = "o4-mini")]
    GPT_o4_mini
}