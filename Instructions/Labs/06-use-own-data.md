---
lab:
    title: 'Use your own data with Azure OpenAI'
---

# Use your own data with Azure OpenAI

The Azure OpenAI Service enables you to use your own data with the intelligence of the underlying LLM. You can limit the model to only use your data for pertinent topics, or blend it with results from the pre-trained model.

This exercise will take approximately **20** minutes.

## Before you start

You will need an Azure subscription that has been approved for access to the Azure OpenAI service. 

- To sign up for a free Azure subscription, visit [https://azure.microsoft.com/free](https://azure.microsoft.com/free).
- To request access to the Azure OpenAI service, visit [https://aka.ms/oaiapply](https://aka.ms/oaiapply).

## Provision an Azure OpenAI resource

Before you can use Azure OpenAI models, you must provision an Azure OpenAI resource in your Azure subscription.

1. Sign into the [Azure portal](https://portal.azure.com?azure-portal=true).
2. Create an **Azure OpenAI** resource with the following settings:
    - **Subscription**: An Azure subscription that has been approved for access to the Azure OpenAI service.
    - **Resource group**: Choose an existing resource group, or create a new one with a name of your choice.
    - **Region**: Choose any available region.
    - **Name**: A unique name of your choice.
    - **Pricing tier**: Standard S0
3. Wait for deployment to complete. Then go to the deployed Azure OpenAI resource in the Azure portal.

## Deploy a model

To chat with the Azure OpenAI, you must first deploy a model to use through the **Azure OpenAI Studio**. Once deployed, we will use the model with the playground and use our data to ground its responses.

1. On the **Overview** page for your Azure OpenAI resource, use the **Explore** button to open Azure OpenAI Studio in a new browser tab. Alternatively, navigate to [Azure OpenAI Studio](https://oai.azure.com/?azure-portal=true) directly.
2. In Azure OpenAI Studio, on the **Deployments** page, view your existing model deployments. If you don't already have one, create a new deployment of the **gpt-35-turbo-16k** model with the following settings:
    - **Model**: gpt-35-turbo-16k
    - **Model version**: Auto-update to default
    - **Deployment name**: *A unique name of your choice*
    - **Advanced options**
        - **Content filter**: Default
        - **Tokens per minute rate limit**: 5K\*
        - **Enable dynamic quota**: Enabled

    > \* A rate limit of 5,000 tokens per minute is more than adequate to complete this exercise while leaving capacity for other people using the same subscription.

> **Note**: In some regions, the new model deployment interface doesn't show the **Model version** option. In this case, don't worry and continue without setting the option

## Observe normal chat behavior without adding your own data

Before connecting Azure OpenAI to your data, first observe how the base model responds to queries without any grounding data.

1. Navigate to the **Chat** playground, and make sure the model you deployed is selected in the **Configuration** pane (this should be the default, if you only have one deployed model).
1. Enter the following prompts, and observe the output.

    ```code
    I'd like to take a trip to New York. Where should I stay?
    ```

    ```code
    What are some facts about New York?
    ```

1. Try similar questions about tourism and places to stay for other locations that will be included in our grounding data, such as London, or San Francisco. You'll likely get complete responses about areas or neighborhoods, and some general facts about the city.

## Connect your data in the chat playground

Next, add your data in the chat playground to see how it responds with your data as grounding

1. [Download the data](https://aka.ms/own-data-brochures) that you will use from GitHub. Extract the PDFs in the `.zip` provided.
1. Navigate to the **Chat** playground, and select *Add your data* in the Assistant setup pane.
1. Select **Add a data source** and choose *Upload files* from the dropdown.
1. You'll need to create a storage account and Azure Cognitive Search resource. Under the dropdown for the storage resource, select **Create a new Azure Blob storage resource**, and create a storage account with the following settings. Anything not specified leave as the default.

    - **Subscription**: *Same subscription as your Azure OpenAI resource*
    - **Resource group**: *Same resource group as your Azure OpenAI resource*
    - **Storage account name**: *Enter globally unique name*
    - **Region**: *Same region as your Azure OpenAI resource*
    - **Redundancy**: Locally-redundant storage (LRS)

1. Once the resource is being created, come back to Azure OpenAI Studio and select **Create a new Azure Cognitive Search resource** with the following settings. Anything not specified leave as the default.

    - **Subscription**: *Same subscription as your Azure OpenAI resource*
    - **Resource group**: *Same resource group as your Azure OpenAI resource*
    - **Service name**: *Enter globally unique name*
    - **Location**: *Same location as your Azure OpenAI resource*
    - **Pricing tier**: Basic

1. Wait until your search resource has been deployed, then switch back to the Azure AI Studio and refresh the page.
1. In the **Add data**, enter the following values for your data source, then select **Next**.

    - **Select data source**: Upload files
    - **Select Azure Blob storage resouce**: *Choose the storage resource you created*
        - Turn on CORS when prompted
    - **Select Azure Cognitive Search resource**: *Choose the search resource you created*
    - **Enter the index name**: margiestravel
    - **Add vector search to this search resource**: unchecked
    - **I acknowledge that connecting to an Azure Cognitive Search account will incur usage to my account** : checked

1. On the **Upload files** page, upload the PDFs you downloaded, and then select **Next**.
1. On the **Data management** page select the **Keyword** search type from the drop-down, and then select **Next**.
1. On the **Review and finish** page select **Save and close**, which will add your data. This may take a few minutes, during which you need to leave your window open. Once complete, you'll see the data source, search resource, and index specified in the **Assistant setup** pane.

## Chat with a model grounded in your data

Now that you've added your data, ask the same questions as you did previously, and see how the response differs.

```
I'd like to take a trip to New York. Where should I stay?
```

```
What are some facts about New York?
```

You'll notice a very different response this time, with specifics about certain hotels and a mention of Margie's Travel, as well as references to where the information provided came from. If you open the PDF reference listed in the response, you'll see the same hotels as the model provided.

Try asking it about other cities included in the grounding data, which are Dubai, Las Vegas, London, and San Francisco.

> **Note**: **Add your data** is still in preview and might not always behave as expected for this feature, such as giving the incorrect reference for a city not included in the grounding data.

## Connect your app to your own data

Next, explore how to connect your app to use your own data.

### Set up an application in Cloud Shell

To show how to connect an Azure OpenAI app to your own data, we'll use a short command-line application that runs in Cloud Shell on Azure. Open up a new browser tab to work with Cloud Shell.

1. In the [Azure portal](https://portal.azure.com?azure-portal=true), select the **[>_]** (*Cloud Shell*) button at the top of the page to the right of the search box. A Cloud Shell pane will open at the bottom of the portal.

    ![Screenshot of starting Cloud Shell by clicking on the icon to the right of the top search box.](../media/cloudshell-launch-portal.png#lightbox)

2. The first time you open the Cloud Shell, you may be prompted to choose the type of shell you want to use (*Bash* or *PowerShell*). Select **Bash**. If you don't see this option, skip the step.  

3. If you're prompted to create storage for your Cloud Shell, select **Show advanced settings** and select the following settings:
    - **Subscription**: Your subscription
    - **Cloud shell regions**: Choose any available region
    - **Show VNET isolation setings** Unselected
    - **Resource group**: Use the existing resource group where you provisioned your Azure OpenAI resource
    - **Storage account**: Create a new storage account with a unique name
    - **File share**: Create a new file share with a unique name

    Then wait a minute or so for the storage to be created.

    > **Note**: If you already have a cloud shell set up in your Azure subscription, you may need to use the **Reset user settings** option in the ⚙️ menu to ensure the latest versions of Python and the .NET Framework are installed.

4. Make sure the type of shell indicated on the top left of the Cloud Shell pane is *Bash*. If it's *PowerShell*, switch to *Bash* by using the drop-down menu.

5. Once the terminal starts, enter the following command to download the sample application and save it to a folder called `azure-openai`.

    ```bash
    rm -r azure-openai -f
    git clone https://github.com/MicrosoftLearning/mslearn-openai azure-openai
    ```

6. The files are downloaded to a folder named **azure-openai**. Navigate to the lab files for this exercise using the following command.

    ```bash
    cd azure-openai/Labfiles/06-use-own-data
    ```

7. Open the built-in code editor by running the following command:

    ```bash
    code .
    ```

## Configure your application

For this exercise, you'll complete some key parts of the application to enable using your Azure OpenAI resource. Applications for both C# and Python have been provided. Both apps feature the same functionality.

1. In the code editor, expand the **CSharp** or **Python** folder, depending on your language preference.

2. Open the configuration file for your language.

    - C#: `appsettings.json`
    - Python: `.env`
    
3. Update the configuration values to include:
    - The  **endpoint** and a **key** from the Azure OpenAI resource you created (available on the **Keys and Endpoint** page for your Azure OpenAI resource in the Azure portal)
    - The name you specified for your model deployment (available in the **Deployments** page in Azure OpenAI Studio).
    - The endpoint for your search service (the **Url** value on the overview page for your search resource in the Azure portal).
    - A **key** for your search resource (available in the **Keys** page for your search resource in the Azure portal - you can use either of the admin keys)
    - The name of the search index (which should be `margiestravel`).
    
4. Save the updated configuration file.

5. In the console pane, enter the following commands to navigate to the folder for your preferred language and install the necessary packages.

    **C#**

    ```bash
    cd CSharp
    dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.9
    ```

    **Python**

    ```bash
    cd Python
    pip install python-dotenv
    pip install openai==0.28.1
    ```

6. In the code editor, navigate to your preferred language folder, select the code file, and add the necessary libraries.

    **C#**: OwnData.cs

    ```csharp
   // Add Azure OpenAI package
   using Azure.AI.OpenAI;
    ```

    **Python**: ownData.py

    ```python
   # Add OpenAI import
   import openai
    ```

7. Review the code file, specifically where the search values are used when completing the parameters for the API call.

## Run your application

Now that your app has been configured, run it to send your request to your model and observe the response. You'll notice the response is now grounded in your data similarly to the studio experience.

1. In the Cloud Shell bash terminal, navigate to the folder for your preferred language.
1. Expand the terminal to take up most of your browser window and run the application.

    - **C#**: `dotnet run`
    - **Python**: `python ownData.py`

1. Submit the prompt `Tell me about New York`, and you should see the response referencing your data.

## Clean up

When you're done with your Azure OpenAI resource, remember to delete the resource in the [Azure portal](https://portal.azure.com/?azure-portal=true). Be sure to also include the storage account and search resource, as those can incur a relatively large cost.
