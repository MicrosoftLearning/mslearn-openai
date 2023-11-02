using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;

// Add Azure OpenAI package

  
// Get configuration settings  
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string oaiEndpoint = config["AzureOAIEndpoint"] ?? "";
string oaiKey = config["AzureOAIKey"] ?? "";
string oaiModelName = config["AzureOAIModelName"] ?? "";
string azureSearchEndpoint = config["AzureSearchEndpoint"] ?? "";
string azureSearchKey = config["AzureSearchKey"] ?? "";
string azureSearchIndex = config["AzureSearchIndex"] ?? "";

// Initialize the Azure OpenAI client (Add code here)
OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

string text = "Where should I stay in London?";

// Send request to Azure OpenAI model  
Console.WriteLine("...Sending the following request to Azure OpenAI endpoint...");  
Console.WriteLine("Request: " + text + "\n");  

// Build completion options object
ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
{
    Messages =
    {
        new ChatMessage(ChatRole.User, text),
    },
    MaxTokens = 600,
    Temperature = 0.9f,
    AzureExtensionsOptions = new AzureChatExtensionsOptions()
    {
        Extensions =
        {
            new AzureCognitiveSearchChatExtensionConfiguration()
            {
                SearchEndpoint = new Uri(azureSearchEndpoint),
                SearchKey = new AzureKeyCredential(azureSearchKey),
                IndexName = azureSearchIndex,
            },
        }
    }
};

// Send request to Azure OpenAI model (Add code here)
ChatCompletions response = client.GetChatCompletions(
    deploymentOrModelName: oaiModelName, 
    chatCompletionsOptions);

ChatMessage responseMessage = response.Choices[0].Message;

// Print response
Console.WriteLine("Response: " + responseMessage.Content + "\n");
Console.WriteLine($"Context information from chat extensions:");
foreach (ChatMessage contextMessage in responseMessage.AzureExtensionsContext.Messages)
{
    string contextContent = contextMessage.Content;
    try
    {
        var contextMessageJson = JsonDocument.Parse(contextMessage.Content);
        contextContent = JsonSerializer.Serialize(contextMessageJson, new JsonSerializerOptions()
        {
            WriteIndented = true,
        });
    }
    catch (JsonException)
    {}
    Console.WriteLine($"{contextMessage.Role}: {contextContent}");
}
