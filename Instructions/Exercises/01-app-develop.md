---
lab:
    title: 'Application development with Azure OpenAI Service'
---

# Application development with Azure OpenAI Service

With the Azure OpenAI Service, developers can create chatbots and other applications that excel at understanding natural human language through the use of REST APIs or language specific SDKs. When working with these language models, how developers shape their prompt greatly impacts how the generative AI model will respond. Azure OpenAI models are able to tailor and format content, if requested in a clear and concise way. In this exercise, you'll learn how to connect your application to Azure OpenAI and see how different prompts for similar content help shape the AI model's response to better satisfy your requirements.

In the scenario for this exercise, you will perform the role of a software developer working on a wildlife marketing campaign. You are exploring how to use generative AI to improve advertising emails and categorize articles that might apply to your team. The prompt engineering techniques used in the exercise can be applied similarly for a variety of use cases.

This exercise will take approximately **30** minutes.

## Provision an Azure OpenAI resource

If you don't already have one, provision an Azure OpenAI resource in your Azure subscription.

1. Sign into the **Azure portal** at `https://portal.azure.com`.

1. Create an **Azure OpenAI** resource with the following settings:
    - **Subscription**: *Select an Azure subscription that has been approved for access to the Azure OpenAI service*
    - **Resource group**: *Choose or create a resource group*
    - **Region**: *Make a **random** choice from any of the following regions*\*
        - Canada East
        - East US
        - East US 2
        - France Central
        - Japan East
        - North Central US
        - Sweden Central
        - Switzerland North
        - UK South
    - **Name**: *A unique name of your choice*
    - **Pricing tier**: Standard S0

    > \* Azure OpenAI resources are constrained by regional quotas. The listed regions include default quota for the model type(s) used in this exercise. Randomly choosing a region reduces the risk of a single region reaching its quota limit in scenarios where you are sharing a subscription with other users. In the event of a quota limit being reached later in the exercise, there's a possibility you may need to create another resource in a different region.

3. Wait for deployment to complete. Then go to the deployed Azure OpenAI resource in the Azure portal.

## Deploy a model

Next, you will deploy an Azure OpenAI model resource from the CLI. Refer to this example and replace the following variables with your own values from above:

```dotnetcli
az cognitiveservices account deployment create \
   -g *Your resource group* \
   -n *Name of your OpenAI service* \
   --deployment-name gpt-35-turbo \
   --model-name gpt-35-turbo \
   --model-version 0125  \
   --model-format OpenAI \
   --sku-name "Standard" \
   --sku-capacity 5
```

    > \* Sku-capacity is measured in thousands of tokens per minute. A rate limit of 5,000 tokens per minute is more than adequate to complete this exercise while leaving capacity for other people using the same subscription.

> [!NOTE]
> If you see a warnings about net7.0 framework being out of support, you can disregard them for this exercise.

## Configure your application

Applications for both C# and Python have been provided, and both apps feature the same functionality. First, you'll complete some key parts of the application to enable using your Azure OpenAI resource with asynchronous API calls.

1. In Visual Studio Code, in the **Explorer** pane, browse to the **Labfiles/01-app-develop** folder and expand the **CSharp** or **Python** folder depending on your language preference. Each folder contains the language-specific files for an app into which you're you're going to integrate Azure OpenAI functionality.
2. Right-click the **CSharp** or **Python** folder containing your code files and open an integrated terminal. Then install the Azure OpenAI SDK package by running the appropriate command for your language preference:

    **C#**:

    ```
    dotnet add package Azure.AI.OpenAI --version 2.0.0
    ```

    **Python**:

    ```
    pip install openai==1.54.3
    ```

3. In the **Explorer** pane, in the **CSharp** or **Python** folder, open the configuration file for your preferred language

    - **C#**: appsettings.json
    - **Python**: .env
    
4. Update the configuration values to include:
    - The  **endpoint** and a **key** from the Azure OpenAI resource you created (available on the **Keys and Endpoint** page for your Azure OpenAI resource in the Azure portal)
    - The **deployment name** you specified for your model deployment.
5. Save the configuration file.

## Add code to use the Azure OpenAI service

Now you're ready to use the Azure OpenAI SDK to consume your deployed model.

1. In the **Explorer** pane, in the **CSharp** or **Python** folder, open the code file for your preferred language, and replace the comment ***Add Azure OpenAI package*** with code to add the Azure OpenAI SDK library:

    **C#**: Program.cs

    ```csharp
    // Add Azure OpenAI packages
    using Azure.AI.OpenAI;
    using OpenAI.Chat;
    ```

    **Python**: application.py

    ```python
    # Add Azure OpenAI package
    from openai import AsyncAzureOpenAI
    ```

2. In the code file, find the comment ***Configure the Azure OpenAI client***, and add code to configure the Azure OpenAI client:

    **C#**: Program.cs

    ```csharp
    // Configure the Azure OpenAI client
       AzureOpenAIClient azureClient = new (new Uri(oaiEndpoint), new ApiKeyCredential(oaiKey));
        ChatClient chatClient = azureClient.GetChatClient(oaiDeploymentName);
        ChatCompletion completion = chatClient.CompleteChat(
        [
        new SystemChatMessage(systemMessage),
        new UserChatMessage(userMessage),
        ]);
    ```

    **Python**: application.py

    ```python
    # Configure the Azure OpenAI client
    client = AsyncAzureOpenAI(
        azure_endpoint = azure_oai_endpoint, 
        api_key=azure_oai_key,  
        api_version="2024-02-15-preview"
        )
    ```

3. In the function that calls the Azure OpenAI model, under the comment ***Get response from Azure OpenAI***, add the code to format and send the request to the model.

    **C#**: Program.cs

    ```csharp
    // Get response from Azure OpenAI
    Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

    ```

    **Python**: application.py

    ```python
    # Get response from Azure OpenAI
    messages =[
        {"role": "system", "content": system_message},
        {"role": "user", "content": user_message},
    ]
    
    print("\nSending request to Azure OpenAI model...\n")

    # Call the Azure OpenAI model
    response = await client.chat.completions.create(
        model=model,
        messages=messages,
        temperature=0.7,
        max_tokens=800
    )
    ```

4. Save the changes to the code file.

## Run your application

Now that your app has been configured, run it to send your request to your model and observe the response. You'll notice the only difference between the different options is the content of the prompt, all other parameters (such as token count and temperature) remain the same for each request.

1. In the folder of your preferred language, open `system.txt` in Visual Studio Code. For each of the interactions, you'll enter the **System message** in this file and save it. Each iteration will pause first for you to change the system message.
1. In the interactive terminal pane, ensure the folder context is the folder for your preferred language. Then enter the following command to run the application.

    - **C#**: `dotnet run`
    - **Python**: `python application.py`

    > **Tip**: You can use the **Maximize panel size** (**^**) icon in the terminal toolbar to see more of the console text.

1. For the first iteration, enter the following prompts:

    **System message**

    ```prompt
    You are an AI assistant
    ```

    **User message:**

    ```prompt
    Write an intro for a new wildlife Rescue
    ```

1. Observe the output. The AI model will likely produce a good generic introduction to a wildlife rescue.
1. Next, enter the following prompts which specify a format for the response:

    **System message**

    ```prompt
    You are an AI assistant helping to write emails
    ```

    **User message:**

    ```prompt
    Write a promotional email for a new wildlife rescue, including the following: 
    - Rescue name is Contoso 
    - It specializes in elephants 
    - Call for donations to be given at our website
    ```

    > **Tip**: You may find the automatic typing in the VM doesn't work well with multiline prompts. If that is your issue, copy the entire prompt then paste it into Visual Studio Code.

1. Observe the output. This time, you'll likely see the format of an email with the specific animals included, as well as the call for donations.
1. Next, enter the following prompts that additionally specify the content:

    **System message**

    ```prompt
    You are an AI assistant helping to write emails
    ```

    **User message:**

    ```prompt
    Write a promotional email for a new wildlife rescue, including the following: 
    - Rescue name is Contoso 
    - It specializes in elephants, as well as zebras and giraffes 
    - Call for donations to be given at our website 
    \n Include a list of the current animals we have at our rescue after the signature, in the form of a table. These animals include elephants, zebras, gorillas, lizards, and jackrabbits.
    ```

1. Observe the output, and see how the email has changed based on your clear instructions.
1. Next, enter the following prompts where we add details about tone to the system message:

    **System message**

    ```prompt
    You are an AI assistant that helps write promotional emails to generate interest in a new business. Your tone is light, chit-chat oriented and you always include at least two jokes.
    ```

    **User message:**

    ```prompt
    Write a promotional email for a new wildlife rescue, including the following: 
    - Rescue name is Contoso 
    - It specializes in elephants, as well as zebras and giraffes 
    - Call for donations to be given at our website 
    \n Include a list of the current animals we have at our rescue after the signature, in the form of a table. These animals include elephants, zebras, gorillas, lizards, and jackrabbits.
    ```

1. Observe the output. This time you'll likely see the email in a similar format, but with a much more informal tone. You'll likely even see jokes included!
1. For the final iteration, we're deviating from email generation and exploring *grounding context*. Here you provide a simple system message, and change the app to provide the grounding context as the beginning of the user prompt. The app will then append the user input, and extract information from the grounding context to answer our user prompt.
1. Open the file `grounding.txt` and briefly read the grounding context you'll be inserting.
1. In your app immediately after the comment ***Format and send the request to the model*** and before any existing code, add the following code snippet to read text in from `grounding.txt` to augment the user prompt with the grounding context.

    **C#**: Program.cs

    ```csharp
    // Format and send the request to the model
    Console.WriteLine("\nAdding grounding context from grounding.txt");
    string groundingText = System.IO.File.ReadAllText("grounding.txt");
    userMessage = groundingText + userMessage;
    ```

    **Python**: application.py

    ```python
    # Format and send the request to the model
    print("\nAdding grounding context from grounding.txt")
    grounding_text = open(file="grounding.txt", encoding="utf8").read().strip()
    user_message = grounding_text + user_message
    ```

1. Save the file and rerun your app.
1. Enter the following prompts (with the **system message** still being entered and saved in `system.txt`).

    **System message**

    ```prompt
    You're an AI assistant who helps people find information. You'll provide answers from the text provided in the prompt, and respond concisely.
    ```

    **User message:**

    ```prompt
    What animal is the favorite of children at Contoso?
    ```

> **Tip**: If you would like to see the full response from Azure OpenAI, you can set the **printFullResponse** variable to `True`, and rerun the app.

## Clean up

When you're done with your Azure OpenAI resource, remember to delete the deployment or the entire resource in the **Azure portal** at `https://portal.azure.com`.
