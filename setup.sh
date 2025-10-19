#!/bin/bash

# Azure Excel Chat Setup Script for Unix/Linux/macOS

echo "???? Azure Excel Chat - Setup Script"
echo "?????????????????????????????????????????????????????????????"
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
WHITE='\033[1;37m'
GRAY='\033[0;37m'
NC='\033[0m' # No Color

# Check if .NET 9 is installed
echo -e "${YELLOW}?? Checking .NET installation...${NC}"
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version 2>/dev/null)
    if [[ $DOTNET_VERSION == 9.* ]]; then
        echo -e "${GREEN}? .NET 9 is installed: $DOTNET_VERSION${NC}"
    else
        echo -e "${YELLOW}??  .NET version: $DOTNET_VERSION${NC}"
        echo -e "${YELLOW}   Recommended: .NET 9.0 or later${NC}"
    fi
else
    echo -e "${RED}? .NET is not installed or not in PATH${NC}"
    echo -e "${RED}   Please install .NET 9 from: https://dotnet.microsoft.com/download/dotnet/9.0${NC}"
    exit 1
fi

echo ""

# Initialize user secrets
echo -e "${YELLOW}?? Initializing user secrets...${NC}"
if dotnet user-secrets init &>/dev/null; then
    echo -e "${GREEN}? User secrets initialized${NC}"
else
    echo -e "${RED}? Failed to initialize user secrets${NC}"
    exit 1
fi

echo ""

# Prompt for Azure OpenAI configuration
echo -e "${CYAN}?? Azure OpenAI Configuration${NC}"
echo -e "${GRAY}?????????????????????????????????????????????????????????????${NC}"

# API Key
echo ""
echo -e "${WHITE}Please enter your Azure OpenAI API Key:${NC}"
echo -e "${GRAY}(You can find this in Azure Portal > Your OpenAI Resource > Keys and Endpoint)${NC}"
read -s -p "API Key: " API_KEY
echo ""

if [ -z "$API_KEY" ]; then
    echo -e "${RED}? API Key is required${NC}"
    exit 1
fi

# Endpoint
echo ""
echo -e "${WHITE}Please enter your Azure OpenAI Endpoint:${NC}"
echo -e "${GRAY}(Format: https://your-resource.openai.azure.com/)${NC}"
read -p "Endpoint: " ENDPOINT

if [ -z "$ENDPOINT" ]; then
    echo -e "${RED}? Endpoint is required${NC}"
    exit 1
fi

# Ensure endpoint ends with /
if [[ ! "$ENDPOINT" == */ ]]; then
    ENDPOINT="${ENDPOINT}/"
fi

# Deployment Name
echo ""
echo -e "${WHITE}Please enter your GPT model deployment name:${NC}"
echo -e "${GRAY}(e.g., gpt-4, gpt-35-turbo, or your custom deployment name)${NC}"
read -p "Deployment Name: " DEPLOYMENT_NAME

if [ -z "$DEPLOYMENT_NAME" ]; then
    echo -e "${RED}? Deployment name is required${NC}"
    exit 1
fi

# Optional Excel file path
echo ""
echo -e "${WHITE}Excel file path (optional):${NC}"
echo -e "${GRAY}(Leave empty to use default: ./employees.xlsx)${NC}"
read -p "Excel File Path: " EXCEL_PATH

echo ""
echo -e "${YELLOW}?? Saving configuration...${NC}"

# Set user secrets
if dotnet user-secrets set "AZURE_OPENAI_API_KEY" "$API_KEY" &>/dev/null && \
   dotnet user-secrets set "AZURE_OPENAI_ENDPOINT" "$ENDPOINT" &>/dev/null && \
   dotnet user-secrets set "AZURE_OPENAI_DEPLOYMENT_NAME" "$DEPLOYMENT_NAME" &>/dev/null; then
    
    if [ ! -z "$EXCEL_PATH" ]; then
        dotnet user-secrets set "EXCEL_FILE_PATH" "$EXCEL_PATH" &>/dev/null
    fi
    
    echo -e "${GREEN}? Configuration saved successfully!${NC}"
else
    echo -e "${RED}? Failed to save configuration${NC}"
    exit 1
fi

echo ""

# Restore packages
echo -e "${YELLOW}?? Restoring NuGet packages...${NC}"
if dotnet restore &>/dev/null; then
    echo -e "${GREEN}? Packages restored successfully!${NC}"
else
    echo -e "${RED}? Failed to restore packages${NC}"
    exit 1
fi

echo ""

# Display configuration summary
echo -e "${CYAN}?? Configuration Summary${NC}"
echo -e "${GRAY}?????????????????????????????????????????????????????????????${NC}"
API_KEY_MASKED="$(printf '*%.0s' $(seq 1 $((${#API_KEY}-4))))${API_KEY: -4}"
echo -e "${WHITE}API Key: $API_KEY_MASKED${NC}"
echo -e "${WHITE}Endpoint: $ENDPOINT${NC}"
echo -e "${WHITE}Deployment: $DEPLOYMENT_NAME${NC}"
if [ ! -z "$EXCEL_PATH" ]; then
    echo -e "${WHITE}Excel Path: $EXCEL_PATH${NC}"
else
    echo -e "${WHITE}Excel Path: ./employees.xlsx (default)${NC}"
fi

echo ""

# Test configuration
echo -e "${YELLOW}?? Testing configuration...${NC}"
echo -e "${WHITE}You can now run the application with: dotnet run${NC}"

echo ""
echo -e "${GREEN}?? Setup completed successfully!${NC}"
echo ""
echo -e "${CYAN}Next steps:${NC}"
echo -e "${WHITE}1. Run 'dotnet run' to start the application${NC}"
echo -e "${WHITE}2. Try asking questions like:${NC}"
echo -e "${GRAY}   - 'Who are the engineers?'${NC}"
echo -e "${GRAY}   - 'What is the average salary?'${NC}"
echo -e "${GRAY}   - 'How many people work in Sales?'${NC}"
echo ""
echo -e "${WHITE}For help and documentation, see README.md${NC}"
echo ""

# Clear sensitive variables
unset API_KEY
unset API_KEY_MASKED