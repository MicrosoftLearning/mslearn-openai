// Implicit using statements are included
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;

// Add Azure OpenAI package
using Azure.AI.OpenAI;

// Build a config object and retrieve user settings.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string? oaiEndpoint = config["AzureOAIEndpoint"];
string? oaiKey = config["AzureOAIKey"];
string? oaiModelName = config["AzureOAIModelName"];

// Read sample text file into a string
string textToSummarize = System.IO.File.ReadAllText(@"../text-files/sample-text.txt");

// Generate summary from Azure OpenAI
GetSummaryFromOpenAI(textToSummarize);
    
void GetSummaryFromOpenAI(string text)  
{   
    Console.WriteLine("\nSending request for summary to Azure OpenAI endpoint...\n\n");

    if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiModelName) )
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    // Initialize the Azure OpenAI client
    OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

    // Build completion options object
    CompletionsOptions completionsOptions = new CompletionsOptions()
    {
        Prompt = {
            text
        },
        MaxTokens = 60,
        Temperature = 0.8f,
    };

    // Send request to Azure OpenAI model
    Completions completionsResponse = client.GetCompletions(oaiModelName, completionsOptions);
    string completion = completionsResponse.Choices[0].Text;
    Console.WriteLine($"Chatbot: {completion}");
}  