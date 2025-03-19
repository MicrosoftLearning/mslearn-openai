// Implicit using statements are included
using System.Text;
using System.ClientModel;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Azure;

// Add Azure OpenAI packages


// Build a config object and retrieve user settings.
class ChatMessageLab
{

static string? oaiEndpoint;
static string? oaiKey;
static string? oaiDeploymentName;
    static void Main(string[] args)
{
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

oaiEndpoint = config["AzureOAIEndpoint"];
oaiKey = config["AzureOAIKey"];
oaiDeploymentName = config["AzureOAIDeploymentName"];

//Initialize messages list

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
        // Format and send the request to the model

        GetResponseFromOpenAI(systemMessage, userMessage);
    }
} while (true);

}

// Define the function that gets the response from Azure OpenAI endpoint
private static void GetResponseFromOpenAI(string systemMessage, string userMessage)  
{   
    Console.WriteLine("\nSending prompt to Azure OpenAI endpoint...\n\n");

    if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiDeploymentName) )
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

// Configure the Azure OpenAI client



// Get response from Azure OpenAI




}

}
