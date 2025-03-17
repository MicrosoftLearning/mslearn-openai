using System;
using Azure;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

// Add references


namespace dalle_client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Clear the console
            Console.Clear();
            
            try
            {
                // Get config settings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string project_connection = configuration["PROJECT_CONNECTION"];
                string model_deployment = configuration["MODEL_DEPLOYMENT"];

                // Initialize the project client



                // Get an OpenAI client
 


                // Loop until the user types 'quit'
                int imageCount = 0;
                Uri imageUrl;
                string input_text = "";
                while (input_text.ToLower() != "quit")
                {
                    // Get user input
                    Console.WriteLine("Enter the prompt (or type 'quit' to exit):");
                    input_text = Console.ReadLine();
                    if (input_text.ToLower() != "quit")
                    {
                        // Generate an image
                        


                        // Save the image to a file
                        if(imageUrl != null)
                        {
                            imageCount++;
                            string fileName = $"image_{imageCount}.png";
                            await SaveImage(imageUrl, fileName);
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task SaveImage(Uri imageUrl, string fileName)
        {
            // Create the folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, fileName);

            // Download the image
            using (HttpClient client = new HttpClient())
            {
                byte[] image = await client.GetByteArrayAsync(imageUrl);
                File.WriteAllBytes(filePath, image);
            }
            Console.WriteLine("Image saved as " + filePath);


        }

    }
}

