using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace generate_image
{
    class Program
    {
        private static string? aoaiEndpoint;
        private static string? aoaiKey;
        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                aoaiEndpoint = configuration["AzureOAIEndpoint"] ?? "";
                aoaiKey = configuration["AzureOAIKey"] ?? "";

                // Get prompt for image to be generated
                Console.Clear();
                Console.WriteLine("Enter a prompt to request an image:");
                string prompt = Console.ReadLine() ?? "";

                // Call the DALL-E model
                using (var client = new HttpClient())
                {
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    var api = "openai/deployments/dalle3/images/generations?api-version=2024-02-15-preview";
                    client.BaseAddress = new Uri(aoaiEndpoint);
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    client.DefaultRequestHeaders.Add("api-key", aoaiKey);
                    var data = new
                    {
                        prompt=prompt,
                        n=1,
                        size="1024x1024"
                    };

                    var jsonData = JsonSerializer.Serialize(data);
                    var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(api, contentData); 

                    // Get the revised prompt and image URL from the response
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    JsonNode contentNode = JsonNode.Parse(stringResponse)!;
                    JsonNode dataCollectionNode = contentNode!["data"];
                    JsonNode dataNode = dataCollectionNode[0]!;
                    JsonNode revisedPrompt = dataNode!["revised_prompt"];
                    JsonNode url = dataNode!["url"];
                    Console.WriteLine(revisedPrompt.ToJsonString());
                    Console.WriteLine(url.ToJsonString().Replace(@"\u0026", "&"));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
