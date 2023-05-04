---
lab:
    title: 'Utilize prompt engineering in your app'
---

# Utilize prompt engineering in your app

With the Azure OpenAI Service, developers can create chatbots, language models, and other applications that excel at understanding natural human language. The Azure OpenAI provides access to pre-trained AI models, as well as a suite of APIs and tools for customizing and fine-tuning these models to meet the specific requirements of your application. In this exercise, you'll learn how to deploy a model in Azure OpenAI and use it in your own application to summarize text.

When working with the Azure OpenAI Service, how developers shape their prompt greatly impacts how the generative AI model will respond. Azure OpenAI models are able to tailor and format content, if requested in a clear and concise way. In this exercise, you'll learn how different prompts for similar content help shape the AI model's response to better satisfy your requirements.

Imagine you are trying to send out information for a new wildlife rescue, and want to get assistance from a generative AI model.

This exercise will take approximately **30** minutes.

{% include_relative includes/initial-setup.md %}

The files are downloaded to a folder named **azure-openai**. Navigate to the lab files for this exercise using the following command.

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

2. Open the configuration file for your language

    - C#: `appsettings.json`
    - Python: `.env`
    
3. Update the configuration values to include the **endpoint** and **key** from the Azure OpenAI resource you created, as well as the model name that you deployed, `text-turbo`. Save the file.

4. Navigate to the folder for your preferred language and install the necessary packages

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
1. Observe the prompt input, and generated output. This time you'll likely see the email in a similar format, but with a much less informal tone. You'll likely even see jokes included!

Increasing the temperature often causes the response to vary, even when provided the same prompt, due to the increased randomness. You can run it multiple times with different temperature or top_p values to see how that impacts the response to the same prompt.

If you would like to see the full response from Azure OpenAI, you can set the `printFullResponse` variable to `True`, and rerun the app.

## Clean up

When you're done with your Azure OpenAI resource, remember to delete the deployment or the entire resource in the [Azure portal](https://portal.azure.com?azure-portal=true).
