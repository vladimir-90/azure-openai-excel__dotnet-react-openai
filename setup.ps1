# Azure Excel Chat Setup Script

# This script helps you set up Azure Excel Chat with proper configuration

Write-Host "???? Azure Excel Chat - Setup Script" -ForegroundColor Cyan
Write-Host "?????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""

# Check if .NET 9 is installed
Write-Host "?? Checking .NET installation..." -ForegroundColor Yellow
try {
    $dotnetVersion = & dotnet --version 2>$null
    if ($dotnetVersion -match "^9\.") {
        Write-Host "? .NET 9 is installed: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "??  .NET version: $dotnetVersion" -ForegroundColor Yellow
        Write-Host "   Recommended: .NET 9.0 or later" -ForegroundColor Yellow
    }
} catch {
    Write-Host "? .NET is not installed or not in PATH" -ForegroundColor Red
    Write-Host "   Please install .NET 9 from: https://dotnet.microsoft.com/download/dotnet/9.0" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Initialize user secrets
Write-Host "?? Initializing user secrets..." -ForegroundColor Yellow
try {
    & dotnet user-secrets init 2>$null
    Write-Host "? User secrets initialized" -ForegroundColor Green
} catch {
    Write-Host "? Failed to initialize user secrets" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Prompt for Azure OpenAI configuration
Write-Host "?? Azure OpenAI Configuration" -ForegroundColor Cyan
Write-Host "?????????????????????????????????????????????????????????????" -ForegroundColor Gray

# API Key
Write-Host ""
Write-Host "Please enter your Azure OpenAI API Key:" -ForegroundColor White
Write-Host "(You can find this in Azure Portal > Your OpenAI Resource > Keys and Endpoint)" -ForegroundColor Gray
$apiKey = Read-Host -Prompt "API Key" -AsSecureString
$apiKeyPlain = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($apiKey))

if ([string]::IsNullOrWhiteSpace($apiKeyPlain)) {
    Write-Host "? API Key is required" -ForegroundColor Red
    exit 1
}

# Endpoint
Write-Host ""
Write-Host "Please enter your Azure OpenAI Endpoint:" -ForegroundColor White
Write-Host "(Format: https://your-resource.openai.azure.com/)" -ForegroundColor Gray
$endpoint = Read-Host -Prompt "Endpoint"

if ([string]::IsNullOrWhiteSpace($endpoint)) {
    Write-Host "? Endpoint is required" -ForegroundColor Red
    exit 1
}

# Ensure endpoint ends with /
if (-not $endpoint.EndsWith("/")) {
    $endpoint += "/"
}

# Deployment Name
Write-Host ""
Write-Host "Please enter your GPT model deployment name:" -ForegroundColor White
Write-Host "(e.g., gpt-4, gpt-35-turbo, or your custom deployment name)" -ForegroundColor Gray
$deploymentName = Read-Host -Prompt "Deployment Name"

if ([string]::IsNullOrWhiteSpace($deploymentName)) {
    Write-Host "? Deployment name is required" -ForegroundColor Red
    exit 1
}

# Optional Excel file path
Write-Host ""
Write-Host "Excel file path (optional):" -ForegroundColor White
Write-Host "(Leave empty to use default: ./employees.xlsx)" -ForegroundColor Gray
$excelPath = Read-Host -Prompt "Excel File Path"

Write-Host ""
Write-Host "?? Saving configuration..." -ForegroundColor Yellow

# Set user secrets
try {
    & dotnet user-secrets set "AZURE_OPENAI_API_KEY" $apiKeyPlain
    & dotnet user-secrets set "AZURE_OPENAI_ENDPOINT" $endpoint
    & dotnet user-secrets set "AZURE_OPENAI_DEPLOYMENT_NAME" $deploymentName
    
    if (-not [string]::IsNullOrWhiteSpace($excelPath)) {
        & dotnet user-secrets set "EXCEL_FILE_PATH" $excelPath
    }
    
    Write-Host "? Configuration saved successfully!" -ForegroundColor Green
} catch {
    Write-Host "? Failed to save configuration" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Restore packages
Write-Host "?? Restoring NuGet packages..." -ForegroundColor Yellow
try {
    & dotnet restore 2>$null
    Write-Host "? Packages restored successfully!" -ForegroundColor Green
} catch {
    Write-Host "? Failed to restore packages" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Display configuration summary
Write-Host "?? Configuration Summary" -ForegroundColor Cyan
Write-Host "?????????????????????????????????????????????????????????????" -ForegroundColor Gray
Write-Host "API Key: $('*' * ($apiKeyPlain.Length - 4))$($apiKeyPlain.Substring($apiKeyPlain.Length - 4))" -ForegroundColor White
Write-Host "Endpoint: $endpoint" -ForegroundColor White
Write-Host "Deployment: $deploymentName" -ForegroundColor White
if (-not [string]::IsNullOrWhiteSpace($excelPath)) {
    Write-Host "Excel Path: $excelPath" -ForegroundColor White
} else {
    Write-Host "Excel Path: ./employees.xlsx (default)" -ForegroundColor White
}

Write-Host ""

# Test configuration
Write-Host "?? Testing configuration..." -ForegroundColor Yellow
Write-Host "You can now run the application with: dotnet run" -ForegroundColor White

Write-Host ""
Write-Host "?? Setup completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. Run 'dotnet run' to start the application" -ForegroundColor White
Write-Host "2. Try asking questions like:" -ForegroundColor White
Write-Host "   - 'Who are the engineers?'" -ForegroundColor Gray
Write-Host "   - 'What is the average salary?'" -ForegroundColor Gray
Write-Host "   - 'How many people work in Sales?'" -ForegroundColor Gray
Write-Host ""
Write-Host "For help and documentation, see README.md" -ForegroundColor White
Write-Host ""

# Clear sensitive variables
$apiKey = $null
$apiKeyPlain = $null