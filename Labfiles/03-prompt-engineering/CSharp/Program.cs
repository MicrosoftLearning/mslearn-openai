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
string? oaiModelName = config["AzureOAIModelName"];

string command;
bool printFullResponse = false;

do {
    Console.WriteLine("\n1: Basic prompt (no prompt engineering)\n" +
    "2: Prompt with email formatting and basic system message\n" +
    "3: Prompt with formatting and specifying content\n" +
    "4: Prompt adjusting system message to be light and use jokes\n" +
    "\"quit\" to exit the program\n\n" + 
    "Enter a number to select a prompt:");

    command = Console.ReadLine() ?? "";
    
    switch (command) {
        case "1":
            await GetResponseFromOpenAI("../prompts/basic.txt");
            break;
        case "2":
            await GetResponseFromOpenAI("../prompts/email-format.txt");
            break;
        case "3":
            await GetResponseFromOpenAI("../prompts/specify-content.txt");
            break;
        case "4":
            await GetResponseFromOpenAI("../prompts/specify-tone.txt");
            break;
        case "quit":
            Console.WriteLine("Exiting program...");
            break;
        default:
            Console.WriteLine("Invalid input. Please try again.");
            break;
    }
} while (command != "quit");

async Task GetResponseFromOpenAI(string fileText)  
{   
    Console.WriteLine("\nSending prompt to Azure OpenAI endpoint...\n\n");

    if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiModelName) )
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }
    
    // Initialize the Azure OpenAI client
    
    // Read text file into system and user prompts
    string[] prompts = System.IO.File.ReadAllLines(fileText);
    string systemPrompt = prompts[0].Split(":", 2)[1].Trim();
    string userPrompt = prompts[1].Split(":", 2)[1].Trim();

    // Write prompts to console
    Console.WriteLine("System prompt: " + systemPrompt);
    Console.WriteLine("User prompt: " + userPrompt);
    
    // Create chat completion options
    

    
    // Write response full response to console, if requested
    if (printFullResponse)
    {
        Console.WriteLine($"\nFull response: {JsonSerializer.Serialize(completions, new JsonSerializerOptions { WriteIndented = true })}\n\n");
    }

    // Write response to console
    Console.WriteLine($"\nResponse: {completion}\n\n");
}  