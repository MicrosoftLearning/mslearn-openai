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

// Initialize the Azure OpenAI client
OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));


// Get the prompt text
Console.WriteLine("Enter a question:");
string? text = Console.ReadLine();

// Create extension config for own data
AzureCognitiveSearchChatExtensionConfiguration ownDataConfig = new()
{
        SearchEndpoint = new Uri(azureSearchEndpoint),
        IndexName = azureSearchIndex
};
ownDataConfig.SetSearchKey(azureSearchKey);

// Send request to Azure OpenAI model  
Console.WriteLine("...Sending the following request to Azure OpenAI endpoint...");  
Console.WriteLine("Request: " + text + "\n");

ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
{
    Messages =
    {
        new ChatMessage(ChatRole.User, text)
    },
    MaxTokens = 600,
    Temperature = 0.9f,
    DeploymentName = oaiModelName,
    // Specify extension options
    AzureExtensionsOptions = new AzureChatExtensionsOptions()
    {
        Extensions = {ownDataConfig}
    }
};

ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);
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
