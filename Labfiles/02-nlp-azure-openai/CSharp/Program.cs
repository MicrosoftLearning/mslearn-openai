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

// Read sample text file into a string
string textToSummarize = System.IO.File.ReadAllText(@"../text-files/sample-text.txt");

// Generate summary from Azure OpenAI
GetSummaryFromOpenAI(textToSummarize);

void GetSummaryFromOpenAI(string text)
{
    Console.WriteLine("\nSending request for summary to Azure OpenAI endpoint...\n\n");

    if (string.IsNullOrEmpty(oaiEndpoint) || string.IsNullOrEmpty(oaiKey) || string.IsNullOrEmpty(oaiDeploymentName))
    {
        Console.WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    // Initialize the Azure OpenAI client
    OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));

    // Build completion options object
    ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
    {
        Messages =
    {
        //tell the role (ChatRole.System)
        //add examples ChatRole.User >ChatRole.Assistant
        //Ask Question ChatRole.User ?
        new ChatMessage(ChatRole.System, "You are a helpful assistant."),
        new ChatMessage(ChatRole.User, @"Write an advertisement for the lightweight Ultramop mop, which uses patented absorbent materials to clean floors."),
        new ChatMessage(ChatRole.Assistant, @"Welcome to the future of cleaning!
                                        18
                                        19The Ultramop makes light work of even the dirtiest of floors. Thanks to its patented absorbent materials, 
                                        it ensures a brilliant shine. Just look at these features:
                                        20- Lightweight construction, making it easy to use.
                                        21- High absorbency, enabling you to apply lots of clean soapy water to the floor.
                                        22- Great low price.
                                        23
                                        24Check out this and other products on our website at www.contoso.com."),
        new ChatMessage(ChatRole.User, "Summarize the following text in 20 words or less:\n" + text),
    },
        MaxTokens = 800,
        Temperature = 0.9f,
        NucleusSamplingFactor = (float)0.95,
        FrequencyPenalty = 0,
        PresencePenalty = 0,
        DeploymentName = oaiDeploymentName
    };

    // Send request to Azure OpenAI model
    ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);
    string completion = response.Choices[0].Message.Content;

    Console.WriteLine("Summary: " + completion + "\n");

}