using ExcelAnalysisAI.TestData.Console.Employees;
using ExcelAnalysisAI.TestData.Console.Utility;

Console.Write("How many entities to generate ? ");
int entityCount = int.Parse(Console.ReadLine()!);

Console.Write("File path to save these entities (.xslx format) ? ");
string outputFilePath = Console.ReadLine()!;

var data = new TestEmployeeProducer().Generate(entityCount);
new ExcelPersistenceUtility().SaveToFile(data, outputFilePath);

Console.WriteLine($"Successfully saved {entityCount} employees to '{outputFilePath}'");