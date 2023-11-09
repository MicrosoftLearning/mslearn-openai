import os
import requests
import json
from dotenv import load_dotenv

# Add OpenAI import


def main(): 
        
    try:     
        # Get configuration settings 
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_model = os.getenv("AZURE_OAI_MODEL")
        azure_search_endpoint = os.getenv("AZURE_SEARCH_ENDPOINT")
        azure_search_key = os.getenv("AZURE_SEARCH_KEY")
        azure_search_index = os.getenv("AZURE_SEARCH_INDEX")
        
        # Initialize the Azure OpenAI client
        client = AzureOpenAI(
            base_url=f"{azure_oai_endpoint}/openai/deployments/{azure_oai_model}/extensions",
            api_key=azure_oai_key,
            api_version="2023-09-01-preview")

        # Get the prompt
        text = input('\nEnter a question:\n')

        # Create extension config for own data
        extension_config = dict(dataSources = [  
                { 
                    "type": "AzureCognitiveSearch", 
                    "parameters": { 
                        "endpoint":azure_search_endpoint, 
                        "key": azure_search_key, 
                        "indexName": azure_search_index,
                    }
                }]
                )

        # Send request to Azure OpenAI model
        print("...Sending the following request to Azure OpenAI endpoint...")
        print("Request: " + text + "\n")

        response = client.chat.completions.create(
            model = azure_oai_model,
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

        # print data context
        print("\nContext information:\n")
        context = response.choices[0].message.context
        for context_message in context["messages"]:
            context_json = json.loads(context_message["content"])
            print(json.dumps(context_json, indent=2))
        
    except Exception as ex:
        print(ex)


if __name__ == '__main__': 
    main()


