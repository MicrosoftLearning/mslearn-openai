import requests
import time
import os
from dotenv import load_dotenv

def main(): 
        
    try:
        # Get Azure OpenAI Service settings
        load_dotenv()
        api_base = os.getenv("AZURE_OAI_ENDPOINT")
        api_key = os.getenv("AZURE_OAI_KEY")
        api_version = '2024-02-15-preview'
        
        # Get prompt for image to be generated
        prompt = input("\nEnter a prompt to request an image: ")

        # Call the DALL-E model
        url = "{}openai/deployments/dalle3/images/generations?api-version={}".format(api_base, api_version)
        headers= { "api-key": api_key, "Content-Type": "application/json" }
        body = {
            "prompt": prompt,
            "n": 1,
            "size": "1024x1024"
        }
        response = requests.post(url, headers=headers, json=body)

        # Get the revised prompt and image URL from the response
        revised_prompt = response.json()['data'][0]['revised_prompt']
        image_url = response.json()['data'][0]['url']

        # Display the URL for the generated image
        print(revised_prompt)
        print(image_url)
        

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()


