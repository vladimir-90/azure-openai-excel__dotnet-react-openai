namespace AzureExcelChat.Console.InitialSample;

internal static class Prompts
{
    public const string QueryDescription =
@"Given the following Excel worksheet data structure, analyze the user's question and describe what data should be filtered or retrieved.
- Describe the filtering logic clearly
- Mention specific column names and conditions
- Be specific about what data should be returned
- If it's a calculation (like average, sum, count), mention that clearly
- Focus on the most relevant data for the user's question

Schema / Excel Worksheet Structure:
---
{{$schema}}
---

User Question: {{$input}}

Query Description (be specific and actionable)";

    public const string FinalAnswer =
@"Answer the following user's question based ONLY on the provided data from the Excel file.
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

User Question: {{$input}}";
}