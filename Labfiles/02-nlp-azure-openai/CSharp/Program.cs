// Implicit using statements are included
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.AI.OpenAI;

// Add Azure OpenAI package


// Build a config object and retrieve user settings.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string? oaiEndpoint = config["AzureOAIEndpoint"];
string? oaiKey = config["AzureOAIKey"];
string? oaiModelName = config["AzureOAIModelName"];

// Read sample text file into a string
string textToSummarize = File.ReadAllText(@"sample-text.txt");

// Generate summary from Azure OpenAI
GetSummaryFromOpenAi(textToSummarize);
    
void GetSummaryFromOpenAi(string text)  
{   
    Console.WriteLine("\nSending request for summary to Azure OpenAI endpoint...\n\n");

    if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiModelName) )
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    // Add code to build request...
    
}  