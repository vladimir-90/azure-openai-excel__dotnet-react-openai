### Configure Azure OpenAI Credentials

Add into project 'appsettings.azure-openai.json' file with following shape:

{
    "AzureOpenAI": [
        {
            "Endpoint": "...",
            "ApiKey": "..."
            "Models": [
                {
                    "DeploymentName": "...",
                    "ModelType": "..."
                },
                {
                    "DeploymentName": "...",
                    "ModelType": "..."
                },
                ...
            ]
        },
        ...
    ]
}

NOTE: this file is git-ignored to secure sensitive Azure OpenAI credentials