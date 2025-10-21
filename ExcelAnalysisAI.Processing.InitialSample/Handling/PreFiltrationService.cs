using System.Text.RegularExpressions;

namespace ExcelAnalysisAI.Processing.InitialSample.Handling;

internal static class PreFiltrationService
{
    public static List<List<object>> FilterDataBasedOnQuery(List<List<object>> data, string userQuery, string queryDescription)
    {
        var result = new List<List<object>>();
        if (data.Count == 0) return result;

        // Always include headers
        result.Add(data[0]);

        var query = userQuery.ToLower();
        var description = queryDescription.ToLower();

        // Special handling for department listing queries
        if (query.Contains("list") && (query.Contains("department") || query.Contains("departments")) ||
            description.Contains("list") && (description.Contains("department") || description.Contains("departments")) ||
            query.Contains("what") && (query.Contains("department") || query.Contains("departments")) ||
            query.Contains("show") && (query.Contains("department") || query.Contains("departments")))
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
}