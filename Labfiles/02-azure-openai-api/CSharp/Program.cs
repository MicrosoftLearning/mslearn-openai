// Implicit using statements are included
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;

// Add Azure OpenAI package


// Build a config object and retrieve user settings.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string? oaiEndpoint = config["AzureOAIEndpoint"];
string? oaiKey = config["AzureOAIKey"];
string? oaiDeploymentName = config["AzureOAIDeploymentName"];

if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiDeploymentName) )
{
    Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
    return;
}

// Initialize the Azure OpenAI client...



do {
    Console.WriteLine("Enter your prompt text (or type 'quit' to exit): ");
    string? inputText = Console.ReadLine();
    if (inputText == "quit") break;

    // Generate summary from Azure OpenAI
    if (inputText == null) {
        Console.WriteLine("Please enter a prompt.");
        continue;
    }
    
    Console.WriteLine("\nSending request for summary to Azure OpenAI endpoint...\n\n");

    // Add code to send request...



} while (true);
