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

bool printFullResponse = false;

do {
    // Pause for system message update
    Console.WriteLine("-----------\nPausing the app to allow you to change the system prompt.\nPress any key to continue...");
    Console.ReadKey();
    
    Console.WriteLine("\nUsing system message from system.txt");
    string systemMessage = System.IO.File.ReadAllText("system.txt"); 
    systemMessage = systemMessage.Trim();

    Console.WriteLine("\nEnter user message or type 'quit' to exit:");
    string userMessage = Console.ReadLine() ?? "";
    userMessage = userMessage.Trim();
    
    if (systemMessage.ToLower() == "quit" || userMessage.ToLower() == "quit")
    {
        break;
    }
    else if (string.IsNullOrEmpty(systemMessage) || string.IsNullOrEmpty(userMessage))
    {
        Console.WriteLine("Please enter a system and user message.");
        continue;
    }
    else
    {
        await GetResponseFromOpenAI(systemMessage, userMessage);
    }
} while (true);

async Task GetResponseFromOpenAI(string systemMessage, string userMessage)  
{   
    Console.WriteLine("\nSending prompt to Azure OpenAI endpoint...\n\n");

    if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiDeploymentName) )
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }
    
    // Configure the Azure OpenAI client


    // Format and send the request to the model

    
    ChatCompletions completions = response.Value;
    string completion = completions.Choices[0].Message.Content;
    
    // Write response full response to console, if requested
    if (printFullResponse)
    {
        Console.WriteLine($"\nFull response: {JsonSerializer.Serialize(completions, new JsonSerializerOptions { WriteIndented = true })}\n\n");
    }

    // Write response to console
    Console.WriteLine($"\nResponse:\n{completion}\n\n");
}  