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
        private static string aoaiEndpoint;
        private static string aoaiKey;
        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                aoaiEndpoint = configuration["AzureOAIEndpoint"];
                aoaiKey = configuration["AzureOAIKey"];

                // Get prompt for image to be generated
                Console.Clear();
                string prompt = "";
                Console.WriteLine("Enter a prompt to request an image:");
                prompt = Console.ReadLine();

                // Make the initial call to start the job
                using (var client = new HttpClient())
                {
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    var api = "openai/images/generations:submit?api-version=2023-06-01-preview";
                    client.BaseAddress = new Uri(aoaiEndpoint);
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                    client.DefaultRequestHeaders.Add("api-key", aoaiKey);
                    var data = new
                    {
                        prompt=prompt,
                        n=1,
                        size="512x512"
                    };

                    var jsonData = JsonSerializer.Serialize(data);
                    var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var init_response = await client.PostAsync(api, contentData); 

                    // Get the operation-location URL for the callback
                    var callback_url = init_response.Headers.GetValues("operation-location").FirstOrDefault();

                    // Poll the callback URL until the job has succeeeded (or 100 attempts)
                    var response = await client.GetAsync(callback_url); 
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var status = JsonSerializer.Deserialize<Dictionary<string,object>>(stringResponse)["status"];
                    var tries = 1;
                    while (status.ToString() != "succeeded" && tries < 101)
                    {
                        tries ++;
                        response = await client.GetAsync(callback_url);
                        stringResponse = await response.Content.ReadAsStringAsync();
                        status = JsonSerializer.Deserialize<Dictionary<string,object>>(stringResponse)["status"];
                        Console.WriteLine(tries.ToString() + ": " + status);
                    }

                    // Get the results
                    stringResponse = await response.Content.ReadAsStringAsync();
                    JsonNode contentNode = JsonNode.Parse(stringResponse)!;
                    JsonNode resultNode = contentNode!["result"];
                    JsonNode dataNode = resultNode!["data"];
                    JsonNode urlNode = dataNode[0]!;
                    JsonNode url = urlNode!["url"];

                    // Display the URL for the generated image
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
