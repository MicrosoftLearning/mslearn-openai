// Implicit using statements are included
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

// Build a config object and retrieve user settings.
IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
string? oaiEndpoint = config["AzureOAIEndpoint"];
string? oaiKey = config["AzureOAIKey"];
string? oaiModelName = config["AzureOAIModelName"];

// Read sample text file into a string
string textToSummarize = System.IO.File.ReadAllText(@"..\text-files\sample-text.txt");

// Generate summary from Azure OpenAI
await GetSummaryFromOpenAI(textToSummarize);
    
async Task GetSummaryFromOpenAI(string text)  
{   
    using (var client = new HttpClient())  
    {  
        // Verify that the user has set the required configuration settings.
        if (oaiEndpoint == null || oaiKey == null || oaiModelName == null)
        {
            Console.WriteLine("Missing configuration settings.");
            return;
        }

        // Set up HTTP client
        Console.WriteLine("\nSending request to Azure OpenAI endpoint...\n\n");
        client.BaseAddress = new Uri(oaiEndpoint);
        client.DefaultRequestHeaders.Add("api-key", oaiKey);
        
        // Set up JSON request body, including parameters for Azure OpenAI model
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                prompt = text,
                max_tokens = 60,
                temperature = .8
            }),
            Encoding.UTF8,
            "application/json");
        
        // Send request to Azure OpenAI REST endpoint
        using HttpResponseMessage response = await client.PostAsync(
            "openai/deployments/" + oaiModelName + "/completions?api-version=2022-12-01",
            jsonContent);

        // Parse response
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var results = JsonSerializer.Deserialize<dynamic>(jsonResponse);
        Console.WriteLine("Summary of text: " + 
            results?.GetProperty("choices")[0].GetProperty("text") + "\n");
    } 
}  