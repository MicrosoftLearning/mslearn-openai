using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;

// Add Azure OpenAI package
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Chat;

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
AzureOpenAIClient oaiClient = new AzureOpenAIClient(new Uri(oaiEndpoint), new ApiKeyCredential(oaiKey));

// Get the prompt text
Console.WriteLine("Enter a question:");
string text = Console.ReadLine() ?? "";

// Configure your data source


// Send request to Azure OpenAI model  
Console.WriteLine("...Sending the following request to Azure OpenAI endpoint...");
Console.WriteLine("Request: " + text + "\n");

ChatCompletion response = chatClient.CompleteChat(
    [
    new UserChatMessage(text)],
    chatCompletionOptions);
string responseMessage = response.Content[0].Text;


ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);
ChatResponseMessage responseMessage = response.Choices[0].Message;

// Print response
ChatMessageContext onYourOwnDataContext = response.GetMessageContext();
if (showCitations)
{
    Console.WriteLine($"\n          Citations of data used:");

    if (onYourOwnDataContext?.Intent is not null)
    {
        Console.WriteLine($"Intent: {onYourOwnDataContext.Intent}");
    }

    foreach (ChatCitation citation in onYourOwnDataContext?.Citations ?? [])
    {
        Console.WriteLine(($"                Citation: {citation.Content} - {citation.Uri}"));
    }
}

