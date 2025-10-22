using ExcelAnalysisAI.Web.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddJsonFile("appsettings.azure-openai.json");
builder.Services.AddSingleton(builder.Configuration.GetSection("AzureOpenAI").Get<AzureOpenAIConfig>()!);

// Requests handling

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

// Startup

app.Run();
