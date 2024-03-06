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
            # Prompt for system and user message
            system_text = input("\nEnter system message or type 'quit' to exit: ")
            user_text = input("Enter user message: ")
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

    print("Response: \n\n" + response.choices[0].message.content + "\n")

if __name__ == '__main__': 
    asyncio.run(main())