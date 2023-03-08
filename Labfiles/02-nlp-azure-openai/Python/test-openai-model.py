import os
from dotenv import load_dotenv

# Add OpenAI import
import openai

def main(): 
        
    try: 
    
        # Get configuration settings 
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        
        openai.api_type = "azure"
        openai.api_base = azure_oai_endpoint
        openai.api_version = "2022-12-01"
        openai.api_key = azure_oai_key

        text = open(file="..\\text-files\\sample-text.txt", encoding="utf8").read()
        
        response = openai.Completion.create(
            engine="IBOpenAIDavinci2",
            prompt=text,
            temperature=0.8,
            max_tokens=60,
            top_p=1,
            frequency_penalty=0,
            presence_penalty=0,
            best_of=1,
            stop=None
        )

        print("Summary of text: " + response.choices[0].text + "\n\n")

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()