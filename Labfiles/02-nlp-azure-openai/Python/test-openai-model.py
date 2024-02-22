import os
from dotenv import load_dotenv
# Add Azure OpenAI package
from openai import AzureOpenAI

# Add Azure OpenAI package


def main(): 
        
    try: 
    
        # Get configuration settings 
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_deployment = os.getenv("AZURE_OAI_DEPLOYMENT")

        # Get the directory of the current file
        current_dir = os.path.dirname(os.path.realpath(__file__))

        # Join the directory of the current file with the relative path to your text file
        file_path = os.path.join(current_dir, '../text-files/sample-text.txt')

        # Read text from file
        with open(file=file_path, encoding="utf8") as f:
            text = f.read()

        print("\nSending request for summary to Azure OpenAI endpoint...\n\n")

        # Initialize the Azure OpenAI client
        client = AzureOpenAI(
                azure_endpoint = azure_oai_endpoint, 
                api_key=azure_oai_key,  
                api_version="2023-05-15"
                )

        # Send request to Azure OpenAI model
        response = client.chat.completions.create(
        model=azure_oai_deployment,
        temperature=1.0,
        max_tokens=120,
        messages=[
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": "Summarize the following text in 20 words or less:\n" + text}
        ]
        )

        summary = "Summary: " + response.choices[0].message.content + "\n"
        print(summary)

        # Write the summary to the file
        with open(file_path, 'a') as f:
            f.write(summary)

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()