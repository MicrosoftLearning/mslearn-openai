import os
from dotenv import load_dotenv

# Add Azure OpenAI package
from openai import AzureOpenAI

# Set to True to print the full response from OpenAI for each call
printFullResponse = True


def main():

    try:

        # Get configuration settings
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_deployment = os.getenv("AZURE_OAI_DEPLOYMENT")

        # Configure the Azure OpenAI clientt
        client = AzureOpenAI(
            azure_endpoint=azure_oai_endpoint,
            api_key=azure_oai_key,
            api_version="2023-05-15"
        )

        while True:
            print('1: Basic prompt (no prompt engineering)\n' +
                  '2: Prompt with email formatting and basic system message\n' +
                  '3: Prompt with formatting and specifying content\n' +
                  '4: Prompt adjusting system message to be light and use jokes\n' +
                  '\'quit\' to exit the program\n')
            command = input('Enter a number:')
            if command == '1':
                call_openai_model(messages="../prompts/basic.txt",
                                  model=azure_oai_deployment, client=client)
            elif command == '2':
                call_openai_model(messages="../prompts/email-format.txt",
                                  model=azure_oai_deployment, client=client)
            elif command == '3':
                call_openai_model(messages="../prompts/specify-content.txt",
                                  model=azure_oai_deployment, client=client)
            elif command == '4':
                call_openai_model(messages="../prompts/specify-tone.txt",
                                  model=azure_oai_deployment, client=client)
            elif command.lower() == 'quit':
                print('Exiting program...')
                break
            else:
                print("Invalid input. Please try again.")

    except Exception as ex:
        print(ex)


def call_openai_model(messages, model, client):
    # In this sample, each file contains both the system and user messages
    # First, read them into variables, strip whitespace, then build the messages array
    file = open(file=messages, encoding="utf8")
    system_message = file.readline().split(':', 1)[1].strip()
    user_message = file.readline().split(':', 1)[1].strip()

    # Print the messages to the console
    print("System message: " + system_message)
    print("User message: " + user_message)

    # Format and send the request to the model
    messages = [
        {"role": "system", "content": system_message},
        {"role": "user", "content": user_message},
    ]

    # Call the Azure OpenAI model
    response = client.chat.completions.create(
        model=model,
        messages=messages,
        temperature=0.7,
        max_tokens=800
    )

    if printFullResponse:
        print(response)

    print("Completion: \n\n" + response.choices[0].message.content + "\n")


if __name__ == '__main__':
    main()
