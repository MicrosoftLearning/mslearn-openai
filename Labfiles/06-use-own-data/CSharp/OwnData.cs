using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;

// Add Azure OpenAI package
using Azure.AI.OpenAI;

// Flag to show citations
bool showCitations = false;

// Get configuration settings  
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string oaiEndpoint = config["AzureOAIEndpoint"] ?? "";
string oaiKey = config["AzureOAIKey"] ?? "";
string oaiDeploymentName = config["AzureOAIDeploymentName"] ?? "";
string azureSearchEndpoint = config["AzureSearchEndpoint"] ?? "";
string azureSearchKey = config["AzureSearchKey"] ?? "";
string azureSearchIndex = config["AzureSearchIndex"] ?? "";

// Initialize the Azure OpenAI client
OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

// Get the prompt text
Console.WriteLine("Enter a question:");
string text = Console.ReadLine() ?? "";

// Configure your data source


// Send request to Azure OpenAI model  
Console.WriteLine("...Sending the following request to Azure OpenAI endpoint...");  
Console.WriteLine("Request: " + text + "\n");

ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
{
    Messages =
    {
        new ChatRequestUserMessage(text)
    },
    MaxTokens = 600,
    Temperature = 0.9f,
    DeploymentName = oaiDeploymentName,
    // Specify extension options
    AzureExtensionsOptions = new AzureChatExtensionsOptions()
    {
        Extensions = {ownDataConfig}
    }
};

ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);
ChatResponseMessage responseMessage = response.Choices[0].Message;

// Print response
Console.WriteLine("Response: " + responseMessage.Content + "\n");
Console.WriteLine("  Intent: " + responseMessage.AzureExtensionsContext.Intent);

if (showCitations)
{
    Console.WriteLine($"\n  Citations of data used:");

    foreach (AzureChatExtensionDataSourceResponseCitation citation in responseMessage.AzureExtensionsContext.Citations)
    {
        Console.WriteLine($"    Citation: {citation.Title} - {citation.Url}");
    }
}

