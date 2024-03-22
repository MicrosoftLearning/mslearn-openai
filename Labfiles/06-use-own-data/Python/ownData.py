import os
import json
from dotenv import load_dotenv

# Add OpenAI import
from openai import AzureOpenAI

def main(): 
        
    try:
        # Flag to show citations
        show_citations = False

        # Get configuration settings 
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_deployment = os.getenv("AZURE_OAI_DEPLOYMENT")
        azure_search_endpoint = os.getenv("AZURE_SEARCH_ENDPOINT")
        azure_search_key = os.getenv("AZURE_SEARCH_KEY")
        azure_search_index = os.getenv("AZURE_SEARCH_INDEX")
        
        # Initialize the Azure OpenAI client
        client = AzureOpenAI(
            base_url=f"{azure_oai_endpoint}/openai/deployments/{azure_oai_deployment}/extensions",
            api_key=azure_oai_key,
            api_version="2023-09-01-preview")

        # Get the prompt
        text = input('\nEnter a question:\n')

        # Configure your data source


        # Send request to Azure OpenAI model
        print("...Sending the following request to Azure OpenAI endpoint...")
        print("Request: " + text + "\n")

        response = client.chat.completions.create(
            model = azure_oai_deployment,
            temperature = 0.5,
            max_tokens = 1000,
            messages = [
                {"role": "system", "content": "You are a helpful travel agent"},
                {"role": "user", "content": text}
            ],
            extra_body = extension_config
        )

        # Print response
        print("Response: " + response.choices[0].message.content + "\n")

        if (show_citations):
            # Print citations
            print("Citations:")
            citations = response.choices[0].message.context["messages"][0]["content"]
            citation_json = json.loads(citations)
            for c in citation_json["citations"]:
                print("  Title: " + c['title'] + "\n    URL: " + c['url'])


        
    except Exception as ex:
        print(ex)


if __name__ == '__main__': 
    main()


