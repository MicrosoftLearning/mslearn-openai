import os
from dotenv import load_dotenv

# Add Azure OpenAI package
from openai import AzureOpenAI


def main():

    try:

        # Get configuration settings
        load_dotenv()
        azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
        azure_oai_key = os.getenv("AZURE_OAI_KEY")
        azure_oai_deployment = os.getenv("AZURE_OAI_DEPLOYMENT")

        # Read text from file
        text = open(file="../text-files/sample-text.txt",
                    encoding="utf8").read()

        print("\nSending request for summary to Azure OpenAI endpoint...\n\n")

        # Initialize the Azure OpenAI client
        client = AzureOpenAI(
            azure_endpoint=azure_oai_endpoint,
            api_key=azure_oai_key,
            api_version="2023-05-15"
        )

        # Send request to Azure OpenAI model
        response = client.chat.completions.create(
            model=azure_oai_deployment,
            temperature=0.7,
            max_tokens=120,
            messages=[
                {"role": "system", "content": "You are a helpful assistant."},
                {"role": "user", "content": "Summarize the following text in 20 words or less:\n" + text}
            ]
        )

        print("Summary: " + response.choices[0].message.content + "\n")

    except Exception as ex:
        print(ex)


if __name__ == '__main__':
    main()
