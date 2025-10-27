using ExcelAnalysisAI.Web.Server.Domain;
using ExcelAnalysisAI.Web.Server.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Configuration.AddJsonFile("appsettings.azure-openai.json");
builder.Services.AddSingleton(builder.Configuration.GetSection("AzureOpenAI").Get<List<AzureOpenAIConfig>>()!);

builder.Services.AddTransient<IQueryProcessorFactory, QueryProcessorFactory>();

// Requests handling

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

// Startup

app.Run();