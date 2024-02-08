import time
import asyncio
import json
import os
from openai import AsyncAzureOpenAI
from dotenv import load_dotenv

def main(): 
        
    try:
        # Get Azure OpenAI Service settings        
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_model = os.getenv("AZURE_OAI_MODEL")
        
        api_version = '2023-12-01-preview'
        
        # Get prompt for image to be generated
        prompt = input("\nEnter a subject prompt to request an image: ")

        # Init async client
        client = AsyncAzureOpenAI(
            azure_endpoint=azure_oai_endpoint,
            api_key=azure_oai_key,
            api_version=api_version
        )

        # Async method to generate the image
        async def dall_e(client):
            image = await client.images.generate(
                model=azure_oai_model,
                n=1,
                size="1024x1024",
                prompt=prompt,
            )

            print(image.data[0].url)

        # Run async call
        asyncio.run(dall_e(client))
        

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()


