---
lab:
    title: 'Get started with Azure OpenAI'
---

# Get started with Azure OpenAI Service

Azure OpenAI Service brings the generative AI models developed by OpenAI to the Azure platform, enabling you to develop powerful AI solutions that benefit from the security, scalability, and integration of services provided by the Azure cloud platform. In this exercise, you'll learn how to get started with Azure OpenAI by provisioning the service as an Azure resource and using Azure OpenAI Studio to deploy and explore OpenAI models.

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

## Deploy a model

Azure OpenAI provides a web-based portal named **Azure OpenAI Studio**, that you can use to deploy, manage, and explore models. You'll start your exploration of Azure OpenAI by using Azure OpenAI Studio to deploy a model.

1. On the **Overview** page for your Azure OpenAI resource, use the **Explore** button to open Azure OpenAI Studio in a new browser tab.
2. In Azure OpenAI Studio, create a new deployment with the following settings:
    - **Model name**: text-davinci-003
    - **Deployment name**: text-davinci

> **Note**: Azure OpenAI includes multiple models, each optimized for a different balance of capabilities and performance. In this exercise, you'll start with the **Davinci** model from the **GPT-3** famility of text generation models. **text-davinci-003** is a good general model for summarizing and generating natural language. For more information about the available models in Azure OpneAI, see [Models](https://learn.microsoft.com/azure/cognitive-services/openai/concepts/models) in the Azure OpenAI documentation.

## Explore a model in the playground

The *playground* is a useful interface in Azure OpenAI Studio that you can use to experiment with your deployed models without needing to develop your own client application.

1. In Azure OpenAI Studio, in the left pane under **Playground**, select **GPT-3**.
2. In the **GPT-3 Playground** page, ensure your **text-davinci** deployment is selected and then in the **Examples** list, select **Summarize text**.

    The summarize text sample consists of a *prompt* that provides some text about neutron stars, ending with the line **Tl;dr:** (which stands for "too long, didn't read"). Ending the prompt with this keyword tells the model to summarize the preceding block of text.

3. At the bottom of the page, note the number of *tokens* detected in the text. Tokens are the basic units of a prompt - essentially words or word-parts in the text.
4. Use the **Generate** button to submit the prompt to the model and retrieve a response.

    The response consist of a summary of the original text. The summary should communicate the key points from the original text in less verbose language.

5. Use the **Regenerate** button to resubmit the prompt, and note that the response may vary from the original one. The generative AI model produces new language each time it is called.
6. Under the summarized response, add a new line and enter the following text:

    *How are they formed?*

7. Use the **Generate** button to submit the new prompt and review the response. The previous prompt and response provide context in an ongoing dialog with the model, enabling the model to generate an appropriate answer to your question.
8. Replace the entire contents of the prompt with the following text:

    *Azure OpenAI Service provides REST API access to OpenAI's powerful language models including the GPT-3, Codex and Embeddings model series. These models can be easily adapted to your specific task including but not limited to content generation, summarization, semantic search, and natural language to code translation. Users can access the service through REST APIs, Python SDK, or our web-based interface in the Azure OpenAI Studio.*

    *Tl;dr:*
9. Use the **Generate** button to submit the new prompt and verify that the model summarizes the text appropriately.

## Use a model to classify text

So far, you've seen how to use a model to summarize text. However, the generative models in Azure OpenAI can support a range of different types of task. Let's explore a different example; *text classification*.

1. In the **GPT-3 Playground** page, ensure your **text-davinci** deployment is selected and then in the **Examples** list, select **Classify text**.

    The summatize text sample prompt describes the context for the model in the form of an instruction to classify a news article into one of a range of categories. It then provides the text for the news article (prefixed by *News article:*) and ends with *Classified category:*. The intention is that the model "completes" the final line of the prompt by predicting thre appropriate category.

2. Use the **Generate** button to submit the prompt to the model and retrieve a response. The model should predict an appropriate category for the news article.
3. Under the predicted category, add the following text:

    *news article: Microsoft releases Azure OpenAI service. Microsoft corporation has released an Azure service that makes OpenAI models available for application developers building apps and services in the Azure cloud.*

    *Classified category:*

4. Use the **Generate** button to continue the dialog with the model and generate an appropriate categorization for the new news article.

## Explore prompts and parameters

Up until now, you've based your prompts on examples that are provided in Azure OpenAI Studio. Let's try something different.

1. Replace all of the text in the prompt area with the following text:

    *You are a teacher creating a test for your students.*

    *Write three multiple choice questions based on the following text.*

    *Most computer vision solutions are based on machine learning models that can be applied to visual input from cameras, videos, or images.*

    *\- Image classification involves training a machine learning model to classify images based on their contents. For example, in a traffic monitoring solution you might use an image classification model to classify images based on the type of vehicle they contain, such as taxis, buses, cyclists, and so on.*

    *\- Object detection machine learning models are trained to classify individual objects within an image, and identify their location with a bounding box. For example, a traffic monitoring solution might use object detection to identify the location of different classes of vehicle.*
    
    *\- Semantic segmentation is an advanced machine learning technique in which individual pixels in the image are classified according to the object to which they belong. For example, a traffic monitoring solution might overlay traffic images with "mask" layers to highlight different vehicles using specific colors.*

2. In the **Parameters** pane, set the following parameter values:
    - **Temperature**: 0
    - **Max length (tokens)**: 500
    - **Pre-response text**: Auto-generated questions. Validate before using in a test:
3. Use the **Generate** button to submit the prompt and review the results, which should consist of the value in the *pre-response text* parameter followed by multiple-choice questions that a teacher could use to test students on the computer vision topics in the prompt. The total response should be smaller than the maximum length you specified as a parameter.

    Observe the following about the prompt and parameters you used:

    - The prompt includes natural language context information that instructs the model on how to behave. Specifically, it indicates that the model should assume the role of a teacher creating a test for students.
    The parameters include *Temperature*, which controls the degree to which response generation includes an element of randomness. The value of **0** used in your submission minimizes randomness, resulting in more predictable responses.

4. Use the **Regenerate** button to regenerate the response. It should be similar to the previous response.
5. Change the **Temperature** parameter value to **0.9** and then use the **Regenerate** button to regenerate the response. This time the increased degree of randomness should result in a different response. However, the questions are more likely to contain inaccuracies than the ones in the previously generated response.

