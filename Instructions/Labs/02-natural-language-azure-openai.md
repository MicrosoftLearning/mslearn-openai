---
lab:
    title: 'Build natural language solutions with Azure OpenAI'
---

create resource
get keys
add import
add details for openai resource
add call to resource

# Build natural language solutions with Azure OpenAI Service

With the Azure OpenAI Service, developers can create chatbots, language models, and other applications that excel at understanding natural human language. The Azure OpenAI provides access to pre-trained AI models, as well as a suite of APIs and tools for customizing and fine-tuning these models to meet the specific requirements of your application. In this exercise, you'll learn how to deploy a model in Azure OpenAI and use it in your own application to summarize text.

This exercise will take approximately **30** minutes.

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

1. On the **Overview** page for your Azure OpenAI resource, use the **Explore** button to open Azure OpenAI Studio in a new browser tab.
2. In Azure OpenAI Studio, create a new deployment with the following settings:
    - **Model name**: text-davinci-003
    - **Deployment name**: text-davinci

> **Note**: Each Azure OpenAI model is optimized for a different balance of capabilities and performance. We'll use the **Davinci** model series in the **GPT-3** model family in this exercise, which is highly capable for language understanding. This exercise only uses a single model, however deployment and usage of other models you deploy will work in the same way.

## Set up an application in Cloud Shell

To show how to integrate with an Azure OpenAI model, we'll use a short command-line application that runs in Cloud Shell on Azure. Open up a new browser tab to work with Cloud Shell.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](../media/cloudshell-launch-portal.png#lightbox)

2. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

3. If you're prompted to create storage for your Cloud Shell, ensure your subscription is specified and select **Create storage**. Then wait a minute or so for the storage to be created.

4. Make sure the type of shell indicated on the top left of the Cloud Shell pane is switched to *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu.

5. Once the terminal starts, enter the following command to download the sample application and save it to a folder called `azure-openai`.

    ```bash
    git clone https://github.com/MicrosoftLearning/mslearn-openai azure-openai
    ```
  
6. The files are downloaded to a folder named **azure-openai**. Navigate to the lab files for this exercise using the following command.

    ```bash
    cd azure-openai/Labfiles/02-nlp-azure-openai
    ```

Applications for both C# and Python have been provided, as well as a sample text file you'll use to test the summarization. Both apps feature the same functionality, but achieve it differently; Python uses the available SDK, and C# uses REST since no SDK is currently available.

Open the built-in code editor, and observe the text file that you'll be summarizing with your model located at `text-files/sample-text.txt`. Use the following command to open the lab files in the code editor.

    ```bash
    code .
    ```

## Configure your application

For this exercise, you'll complete some key parts of the application to enable using your Azure OpenAI resource.

1. In the code editor, expand the **CSharp** or **Python** folder, depending on your language preference.

2. Open the configuration file for your language

    - C#: `appsettings.json`
    - Python: `.env`
    
3. Update the configuration values to include the **endpoint** and **key** from the Azure OpenAI resource you created, as well as the model name that you deployed, `text-davinci`. Save the file.

4. If you're using Python, you'll have to install the library for Azure OpenAI to use the SDK, as well as `dotenv-python`. If you're using C#, you can skip this step.

    ```bash
    pip install dotenv-python
    pip install openai
    ```

    Then, navigate to the **Python** folder, select `test-openai-model.py`, and add the `openai` import.

    ```python
    # Add OpenAI import
    import openai
    ```

5. Open up the application code for your language and add the necessary code for building the request, which specifies the various parameters for your model such as `prompt` and `temperature`.

    **C#**
    ```csharp
    // Set up HTTP client
    client.BaseAddress = new Uri(oaiEndpoint);
    client.DefaultRequestHeaders.Add("api-key", oaiKey);
    
    // Set up JSON request body, including parameters for Azure OpenAI model
    using StringContent jsonContent = new(
        JsonSerializer.Serialize(new
        {
            prompt = text,
            max_tokens = 60,
            temperature = .8
        }),
        Encoding.UTF8,
        "application/json");
    
    // Send request to Azure OpenAI REST endpoint
    using HttpResponseMessage response = await client.PostAsync(
        "openai/deployments/" + oaiModelName + "/completions?api-version=2022-12-01",
        jsonContent);
    ```

    **Python**
    ```python
    # Set OpenAI configuration settings
    openai.api_type = "azure"
    openai.api_base = azure_oai_endpoint
    openai.api_version = "2022-12-01"
    openai.api_key = azure_oai_key

    # Send request to Azure OpenAI model
    print("Sending request for summary to Azure OpenAI endpoint...\n\n")
    response = openai.Completion.create(
        engine=azure_oai_model,
        prompt=text,
        temperature=1,
        max_tokens=60,
        top_p=1,
        frequency_penalty=0,
        presence_penalty=0,
        best_of=1,
        stop=None
    )
    ```

## Run your application

Now that your app has been configured, run it to send your request to your model and observe the response.

1. In the Cloud Shell bash terminal, navigate to the folder for your preferred language.
1. Run the application.

    **C#**: `dotnet run`
    **Python**: `python test-openai-model.py`

1. Observe the summarization of the sample text file.
1. Navigate to your code file for your preferred language, and change the *temperature* value to `1`. Save the file.
1. Run the application again, and observe the output.

Increasing the temperature often causes the summary to vary, even when provided the same text, due to the increased randomness. You can run it several times to see how the output may change. Try using different values for your temperature with the same input.

## Clean up

When you're done with your Azure OpenAI resource, remember to delete the deployment or the entire resource in the [Azure portal](https://portal.azure.com?azure-portal=true).
