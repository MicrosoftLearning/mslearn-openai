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
string? oaiDeploymentName = config["AzureOAIDeploymentName"];

string command;
bool printFullResponse = false;

do {
    Console.WriteLine("\n1: Add comments to my function\n" +
    "2: Write unit tests for my function\n" +
    "3: Fix my Go Fish game\n" +
    "\"quit\" to exit the program\n\n" + 
    "Enter a number to select a task:");

    command = Console.ReadLine() ?? "";

    if(command == "quit") {
        Console.WriteLine("Exiting program...");
        break;
    }

    Console.WriteLine("\nEnter a prompt: ");
    string userPrompt = Console.ReadLine() ?? "";
    string codeFile = "";
    
    if(command == "1" || command == "2")
        codeFile = System.IO.File.ReadAllText("../sample-code/function/function.cs");
    else if(command == "3")
        codeFile = System.IO.File.ReadAllText("../sample-code/go-fish/go-fish.cs");
    else {
        Console.WriteLine("Invalid input. Please try again.");
        continue;
    }

    userPrompt += codeFile;
            
    await GetResponseFromOpenAI(userPrompt);
} while (true);

async Task GetResponseFromOpenAI(string prompt)  
{   
    Console.WriteLine("\nCalling Azure OpenAI to generate code...\n\n");

    if(string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiDeploymentName) )
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }
    
    // Configure the Azure OpenAI client
    OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

    // Define chat prompts
    string systemPrompt = "You are a helpful AI assistant that helps programmers write code.";
    string userPrompt = prompt;

    // Format and send the request to the model


    // Write full response to console, if requested
    if (printFullResponse)
    {
        Console.WriteLine($"\nFull response: {JsonSerializer.Serialize(completions, new JsonSerializerOptions { WriteIndented = true })}\n\n");
    }

    // Write the file.
    System.IO.File.WriteAllText("result/app.txt", completion);

    // Write response to console
    Console.WriteLine($"\nResponse written to result/app.txt\n\n");
}  