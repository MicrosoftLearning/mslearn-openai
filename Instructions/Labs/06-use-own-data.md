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
    - **Resource group**: Create a new resource group with a name of your choice.
    - **Region**: Choose any available region.
    - **Name**: A unique name of your choice.
    - **Pricing tier**: Standard S0
3. Wait for deployment to complete. Then go to the deployed Azure OpenAI resource in the Azure portal.

## Deploy a model

To chat with the Azure OpenAI, you must first deploy a model to use through the **Azure OpenAI Studio**. Once deployed, we will use the model with the playground and use our data to ground its responses.

1. On the **Overview** page for your Azure OpenAI resource, use the **Explore** button to open Azure OpenAI Studio in a new browser tab. Alternatively, navigate to [Azure OpenAI Studio](https://oai.azure.com/?azure-portal=true) directly.
2. In Azure OpenAI Studio, create a new deployment with the following settings:
    - **Model name**: gpt-35-turbo
    - **Model version**: *Use the default version*
    - **Deployment name**: text-turbo

## Observe normal chat behavior without adding your own data

Before connecting Azure OpenAI to your data, first observe how the base model responds to queries without any grounding data.

1. Navigate to the **Chat** playground, and make sure the `gpt-35-turbo` model you deployed is selected in the **Configuration** pane (this should be the default, if you only have one deployed model).
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

```code
I'd like to take a trip to New York. Where should I stay?
```

```code
What are some facts about New York?
```

You'll notice a very different response this time, with specifics about certain hotels and a mention of Margie's Travel, as well as references to where the information provided came from. If you open the PDF reference listed in the response, you'll see the same hotels as the model provided.

Try asking it about other cities included in the grounding data, which are Dubai, Las Vegas, London, and San Francisco.

> **Note**: **Add your data** is still in preview and might not always behave as expected for this feature, such as giving the incorrect reference for a city not included in the grounding data.

## Clean up

When you're done with your Azure OpenAI resource, remember to delete the resource in the [Azure portal](https://portal.azure.com/?azure-portal=true). Be sure to also include the storage account and search resource, as those can incur a relatively large cost.
