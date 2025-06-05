import os
import openai
import dotenv

## Flag to show citations
showCitations = False

dotenv.load_dotenv()

endpoint = os.environ.get("AZURE_OAI_ENDPOINT")
api_key = os.environ.get("AZURE_OAI_KEY")
deployment = os.environ.get("AZURE_OAI_DEPLOYMENT")

client = openai.AzureOpenAI(
    azure_endpoint=endpoint,
    api_key=api_key,
    api_version="2024-02-01",
)

# Configure your data source


print(completion.choices[0].message.content)

if showCitations:
    print(f"\n{completion.choices[0].message.context}")
