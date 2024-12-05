from openai import AzureOpenAI
import os
import requests
from PIL import Image
import json
from dotenv import load_dotenv

 # Get configuration settings 
load_dotenv()
azure_oai_endpoint = os.getenv("AZURE_OAI_ENDPOINT")
azure_oai_key = os.getenv("AZURE_OAI_KEY")
azure_oai_deployment = os.getenv("AZURE_OAI_DEPLOYMENT")


# Configure client
client = AzureOpenAI(
    api_version="2024-02-01",  
    api_key=azure_oai_key,  
    azure_endpoint=azure_oai_endpoint
)


# Read in system message and prompt for user message
user_text = input("Enter user message, or 'quit' to exit: ")
if user_text.lower() == 'quit':
    exit()


result = client.images.generate(
    model=azure_oai_deployment,
    prompt=user_text,
    n=1
)

json_response = json.loads(result.model_dump_json())

# Set the directory for the stored image
image_dir = os.path.join(os.curdir, 'images')

# If the directory doesn't exist, create it
if not os.path.isdir(image_dir):
    os.mkdir(image_dir)

# Initialize the image path (note the filetype should be png)
image_path = os.path.join(image_dir, 'generated_image.png')

# Retrieve the generated image
image_url = json_response["data"][0]["url"]  # extract image URL from response
generated_image = requests.get(image_url).content  # download the image
with open(image_path, "wb") as image_file:
    image_file.write(generated_image)

# Display the image in the default image viewer
image = Image.open(image_path)
image.show()