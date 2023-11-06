import os
import requests
from dotenv import load_dotenv

# Add Azure OpenAI package


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
        
        # Set OpenAI configuration settings 
        openai.api_type = "azure" 
        openai.api_base = azure_oai_endpoint 
        openai.api_version = "2023-08-01-preview" 
        openai.api_key = azure_oai_key 

        # Set up the OpenAI Python SDK to use your own data for the chat endpoint.
        setup_byod(azure_oai_model)

        text = "Where should I stay in London?"

        print("...Sending the following request to Azure OpenAI endpoint...")
        print("Request: " + text + "\n")

        # Call chat completion connection. (Add code here)
        response = openai.ChatCompletion.create(
            deployment_id = azure_oai_model,
            temperature = 0.5,
            max_tokens = 1000,
            messages = [
                {"role": "system", "content": "You are a helpful travel agent"},
                {"role": "user", "content": text}
            ],
            dataSources = [  
                {
                    "type": "AzureCognitiveSearch",
                    "parameters": {
                        "endpoint": azure_search_endpoint,
                        "key": azure_search_key,
                        "indexName": azure_search_index,
                    }
                }
            ]
        )

        # Print response
        print("Response: " + response.choices[0].message.content + "\n")

        # print data context
        print("\nContext information:\n", response.choices[0].message.context)
        
    except Exception as ex:
        print(ex)

def setup_byod(deployment_id: str) -> None:
    """Sets up the OpenAI Python SDK to use your own data for the chat endpoint.
    
    :param deployment_id: The deployment ID for the model to use with your own data.

    To remove this configuration, simply set openai.requestssession to None.
    """

    class BringYourOwnDataAdapter(requests.adapters.HTTPAdapter):

        def send(self, request, **kwargs):
            request.url = f"{openai.api_base}/openai/deployments/{deployment_id}/extensions/chat/completions?api-version={openai.api_version}"
            return super().send(request, **kwargs)

    session = requests.Session()

    # Mount a custom adapter which will use the extensions endpoint for any call using the given `deployment_id`
    session.mount(
        prefix=f"{openai.api_base}/openai/deployments/{deployment_id}",
        adapter=BringYourOwnDataAdapter()
    )

    openai.requestssession = session

if __name__ == '__main__': 
    main()


