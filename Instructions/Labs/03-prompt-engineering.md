---
lab:
    title: 'Utilize prompt engineering in your app'
---

# Utilize prompt engineering in your app

With the Azure OpenAI Service, developers can create chatbots, language models, and other applications that excel at understanding natural human language. The Azure OpenAI provides access to pre-trained AI models, as well as a suite of APIs and tools for customizing and fine-tuning these models to meet the specific requirements of your application. In this exercise, you'll learn how to deploy a model in Azure OpenAI and use it in your own application to summarize text.

When working with the Azure OpenAI Service, how developers shape their prompt greatly impacts how the generative AI model will respond. Azure OpenAI models are able to tailor and format content, if requested in a clear and concise way. In this exercise, you'll learn how different prompts for similar content help shape the AI model's response to better satisfy your requirements.

Imagine you are trying to send out information for a new wildlife rescue, and want to get assistance from a generative AI model.

This exercise will take approximately **25** minutes.

## Before you start

You will need an Azure subscription that has been approved for access to the Azure OpenAI service.

- To sign up for a free Azure subscription, visit [https://azure.microsoft.com/free](https://azure.microsoft.com/free).
- To request access to the Azure OpenAI service, visit [https://aka.ms/oaiapply](https://aka.ms/oaiapply).

## Provision an Azure OpenAI resource

Before you can use Azure OpenAI models, you must provision an Azure OpenAI resource in your Azure subscription.

1. Sign into the [Azure portal](https://portal.azure.com).
2. Create an **Azure OpenAI** resource with the following settings:
    - **Subscription**: An Azure subscription that has been approved for access to the Azure OpenAI service.
    - **Resource group**: Create a new resource group with a name of your choice.
    - **Region**: Choose any available region.
    - **Name**: A unique name of your choice.
    - **Pricing tier**: Standard S0
3. Wait for deployment to complete. Then go to the deployed Azure OpenAI resource in the Azure portal.
4. Navigate to **Keys and Endpoint** page, and save those to a text file to use later.

## Deploy a model

To use the Azure OpenAI API, you must first deploy a model to use through the **Azure OpenAI Studio**. Once deployed, we will reference that model in our app.

1. On the **Overview** page for your Azure OpenAI resource, use the **Explore** button to open Azure OpenAI Studio in a new browser tab. Alternatively, navigate to [Azure OpenAI Studio](https://oai.azure.com/?azure-portal=true) directly.
2. In Azure OpenAI Studio, create a new deployment with the following settings:
    - **Model**: gpt-35-turbo
    - **Model version**: *Use the default version*
    - **Deployment name**: text-turbo

> **Note**: Each Azure OpenAI model is optimized for a different balance of capabilities and performance. We'll use the **3.5 Turbo** model series in the **GPT-3** model family in this exercise, which is highly capable for language understanding. This exercise only uses a single model, however deployment and usage of other models you deploy will work in the same way.

## Apply prompt engineering in chat playground

Before using your app, examine how prompt engineering improves the model response in the playground. In this first example, imagine you are trying to write a python app of animals with fun names.

1. In [Azure OpenAI Studio](https://oai.azure.com/?azure-portal=true), navigate to the **Chat** playground in the left pane.
1. In the **Assistant setup** section at the top, enter `You are a helpful AI assistant` as the system message.
1. In the **Chat session** section, enter the following prompt and press *Enter*.

    ```code
   1. Create a list of animals
   2. Create a list of whimsical names for those animals
   3. Combine them randomly into a list of 25 animal and name pairs
    ```

1. The model will likely respond with an answer to satisfy the prompt, split into a numbered list. This is a good response, but not what we're looking for.
1. Next, update the system message to include instructions `You are an AI assistant helping write python code. Complete the app based on provided comments`. Click **Save changes**
1. Format the instructions as python comments. Send the following prompt to the model.

    ```code
   # 1. Create a list of animals
   # 2. Create a list of whimsical names for those animals
   # 3. Combine them randomly into a list of 25 animal and name pairs
    ```

1. The model should correctly respond with complete python code doing what the comments requested.
1. Next we'll see the impact of few shot prompting when attempting to classify articles. Return to the system message, and enter `You are a helpful AI assistant` again, and save your changes. This will create a new chat session.
1. Send the following prompt to the model.

    ```code
   Severe drought likely in California

   Millions of California residents are bracing for less water and dry lawns as drought threatens to leave a large swath of the region with a growing water shortage.
   
   In a remarkable indication of drought severity, officials in Southern California have declared a first-of-its-kind action limiting outdoor water use to one day a week for nearly 8 million residents.
   
   Much remains to be determined about how daily life will change as people adjust to a drier normal. But officials are warning the situation is dire and could lead to even more severe limits later in the year.
    ```

1. The response will likely be some information about drought in California. While not a bad response, it's not the classification we're looking for.
1. In the **Assistant setup** section near the system message, select the **Add an example** button. Add the following example.

    **User:**

    ```code
   New York Baseballers Wins Big Against Chicago
   
   New York Baseballers mounted a big 5-0 shutout against the Chicago Cyclones last night, solidifying their win with a 3 run homerun late in the bottom of the 7th inning.
   
   Pitcher Mario Rogers threw 96 pitches with only two hits for New York, marking his best performance this year.
   
   The Chicago Cyclones' two hits came in the 2nd and the 5th innings, but were unable to get the runner home to score.
    ```

    **Assistant:**

    ```code
   Sports
    ```

1. Add another example with the following text.

    **User:**

    ```code
   Joyous moments at the Oscars

   The Oscars this past week where quite something!
   
   Though a certain scandal might have stolen the show, this year's Academy Awards were full of moments that filled us with joy and even moved us to tears.
   These actors and actresses delivered some truly emotional performances, along with some great laughs, to get us through the winter.
   
   From Robin Kline's history-making win to a full performance by none other than Casey Jensen herself, don't miss tomorrows rerun of all the festivities.
    ```

    **Assistant:**

    ```code
   Entertainment
    ```

1. Save those changed to the assistant setup, and send the same prompt about California drought, provided here again for convenience.

    ```code
   Severe drought likely in California

   Millions of California residents are bracing for less water and dry lawns as drought threatens to leave a large swath of the region with a growing water shortage.
   
   In a remarkable indication of drought severity, officials in Southern California have declared a first-of-its-kind action limiting outdoor water use to one day a week for nearly 8 million residents.
   
   Much remains to be determined about how daily life will change as people adjust to a drier normal. But officials are warning the situation is dire and could lead to even more severe limits later in the year.
    ```

1. This time the model should respond with an appropriate classification, even without instructions.

## Set up an application in Cloud Shell

To show how to integrate with an Azure OpenAI model, we'll use a short command-line application that runs in Cloud Shell on Azure. Open up a new browser tab to work with Cloud Shell.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](../media/cloudshell-launch-portal.png#lightbox)

2. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

3. If you're prompted to create storage for your Cloud Shell, ensure your subscription is specified and select **Create storage**. Then wait a minute or so for the storage to be created.

4. Make sure the type of shell indicated on the top left of the Cloud Shell pane is switched to *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu.

5. Once the terminal starts, enter the following command to download the sample application and save it to a folder called `azure-openai`.

    ```bash
   rm -r azure-openai -f
   git clone https://github.com/MicrosoftLearning/mslearn-openai azure-openai
    ```

6. The files are downloaded to a folder named **azure-openai**. Navigate to the lab files for this exercise using the following command.

    ```bash
   cd azure-openai/Labfiles/03-prompt-engineering
    ```

    Applications for both C# and Python have been provided, as well as a text files that provide the prompts. Both apps feature the same functionality.

    Open the built-in code editor, and you can observe the prompt files that you'll be using in `prompts`. Use the following command to open the lab files in the code editor.

    ```bash
   code .
    ```

## Configure your application

For this exercise, you'll complete some key parts of the application to enable using your Azure OpenAI resource.

1. In the code editor, expand the **CSharp** or **Python** folder, depending on your language preference.

2. Open the configuration file for your language.

    - C#: `appsettings.json`
    - Python: `.env`
    
3. Update the configuration values to include the **endpoint** and **key** from the Azure OpenAI resource you created, as well as the model name that you deployed, `text-turbo`. Save the file.

4. Navigate to the folder for your preferred language and install the necessary packages.

    **C#**

    ```bash
   cd CSharp
   dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5
    ```

    **Python**

    ```bash
   cd Python
   pip install python-dotenv
   pip install openai
    ```

5. Navigate to your preferred language folder, select the code file, and add the necessary libraries.

    **C#**

    ```csharp
   // Add Azure OpenAI package
   using Azure.AI.OpenAI;
    ```

    **Python**

    ```python
   # Add OpenAI import
   import openai
    ```

5. Open up the application code for your language and add the necessary code for configuring the client.

    **C#**

    ```csharp
   // Initialize the Azure OpenAI client
   OpenAIClient client = new OpenAIClient(new Uri(oaiEndpoint), new AzureKeyCredential(oaiKey));
    ```

    **Python**

    ```python
   # Set OpenAI configuration settings
   openai.api_type = "azure"
   openai.api_base = azure_oai_endpoint
   openai.api_version = "2023-03-15-preview"
   openai.api_key = azure_oai_key
    ```

6. In the function that calls the Azure OpenAI model, add the code to format and send the request to the model.

    **C#**

    ```csharp
   // Create chat completion options
   var chatCompletionsOptions = new ChatCompletionsOptions()
   {
       Messages =
       {
          new ChatMessage(ChatRole.System, systemPrompt),
          new ChatMessage(ChatRole.User, userPrompt)
       },
       Temperature = 0.7f,
       MaxTokens = 800,
   };

   // Get response from Azure OpenAI
   Response<ChatCompletions> response = await client.GetChatCompletionsAsync(
       oaiModelName,
       chatCompletionsOptions
   );

   ChatCompletions completions = response.Value;
   string completion = completions.Choices[0].Message.Content;
    ```

    **Python**

    ```python
   # Build the messages array
   messages =[
       {"role": "system", "content": system_message},
       {"role": "user", "content": user_message},
   ]

   # Call the Azure OpenAI model
   response = openai.ChatCompletion.create(
       engine=model,
       messages=messages,
       temperature=0.7,
       max_tokens=800
   )
    ```

## Run your application

Now that your app has been configured, run it to send your request to your model and observe the response. You'll notice the only difference between the different options is the content of the prompt, all other parameters (such as token count and temperature) remain the same for each request.

Each prompt is displayed in the console as it sends for you to see how differences in prompts produce different responses.

1. In the Cloud Shell bash terminal, navigate to the folder for your preferred language.
1. Run the application, and expand the terminal to take up most of your browser window.

    - **C#**: `dotnet run`
    - **Python**: `python prompt-engineering.py`

1. Choose option **1** for the most basic prompt.
1. Observe the prompt input, and generated output. The AI model will likely produce a good generic introduction to a wildlife rescue.
1. Next, choose option **2** to give it a prompt asking for an intro email, along with some details about the wildlife rescue.
1. Observe the prompt input, and generated output. This time, you'll likely see the format of an email with the specific animals included, as well as the call for donations.
1. Next, choose option **3** to ask for an email similar to above, but with a formatted table with additional animals included.
1. Observe the prompt input, and generated output. This time, you'll likely see a similar email with text formatted in a specific way (in this case, a table near the end) demonstrating how the generative AI models can format output when requested.
1. Next, choose option **4** to ask for a similar email, but this time specifying different tone in the system message.
1. Observe the prompt input, and generated output. This time you'll likely see the email in a similar format, but with a much more informal tone. You'll likely even see jokes included!

Increasing the temperature often causes the response to vary, even when provided the same prompt, due to the increased randomness. You can run it multiple times with different temperature or top_p values to see how that impacts the response to the same prompt.

If you would like to see the full response from Azure OpenAI, you can set the `printFullResponse` variable to `True`, and rerun the app.

## Clean up

When you're done with your Azure OpenAI resource, remember to delete the deployment or the entire resource in the [Azure portal](https://portal.azure.com/?azure-portal=true).
