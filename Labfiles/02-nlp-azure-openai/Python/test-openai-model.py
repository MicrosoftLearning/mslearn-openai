import os
from dotenv import load_dotenv

# Add OpenAI import


def main(): 
        
    try: 
    
        # Get configuration settings 
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_model = os.getenv("AZURE_OAI_MODEL")
        
        # Read text from file
        text = open(file="../text-files/sample-text.txt", encoding="utf8").read()
        
        # Set OpenAI configuration settings
        
        

        print("Summary of text: " + response.choices[0].text + "\n")

    except Exception as ex:
        print(ex)

if __name__ == '__main__': 
    main()