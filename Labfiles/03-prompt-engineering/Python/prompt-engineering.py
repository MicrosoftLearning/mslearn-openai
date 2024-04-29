import os
import asyncio
from dotenv import load_dotenv

# Add Azure OpenAI package


# Set to True to print the full response from OpenAI for each call
printFullResponse = False

async def main(): 
        
    try: 
    
        # Get configuration settings 
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_deployment = os.getenv("AZURE_OAI_DEPLOYMENT")
        
        # Configure the Azure OpenAI client
        

        while True:
            # Pause the app to allow the user to enter the system prompt
            print("------------------\nPausing the app to allow you to change the system prompt.\nPress enter to continue...")
            input()

            # Read in system message and prompt for user message
            system_text = open(file="system.txt", encoding="utf8").read().strip()
            user_text = input("Enter user message, or 'quit' to exit: ")
            if user_text.lower() == 'quit' or system_text.lower() == 'quit':
                print('Exiting program...')
                break
            
            await call_openai_model(system_message = system_text, 
                                    user_message = user_text, 
                                    model=azure_oai_deployment, 
                                    client=client
                                    )

    except Exception as ex:
        print(ex)

async def call_openai_model(system_message, user_message, model, client):
    # Format and send the request to the model
    


    if printFullResponse:
        print(response)

    print("Response:\n" + response.choices[0].message.content + "\n")

if __name__ == '__main__': 
    asyncio.run(main())
