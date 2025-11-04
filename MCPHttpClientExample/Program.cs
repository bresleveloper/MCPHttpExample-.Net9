

using ModelContextProtocol.Client;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using Microsoft.Extensions.AI;
using System.Text.Json;
using System.Runtime.CompilerServices;



Console.WriteLine("MCP Client Started!");


// make sure AspNetCoreMcpServer is running
var clientTransport = new HttpClientTransport(new()
{
    Endpoint = new Uri("http://localhost:4006")
});


// Create MCP Client
await using var mcpClient = await McpClient.CreateAsync(clientTransport!);



// Logger
using var loggerFactory = LoggerFactory.Create(builder =>
    //builder.AddConsole().SetMinimumLevel(LogLevel.Information));
    builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
    //builder.AddConsole().SetMinimumLevel(LogLevel.Trace));


// Configure Ollama LLM Client
var ollamaChatClient = new OllamaApiClient(
    new Uri("http://localhost:11434/"),
    //"llama3.2:latest"
    "qwen3:8b"
);


var chatClient = new RealTimeProgressChatClient(ollamaChatClient) as IChatClient;
chatClient = chatClient.AsBuilder()
        .UseLogging(loggerFactory)
        .UseFunctionInvocation()
        .Build();


// Get available tools from MCP Server
var mcpTools = await mcpClient.ListToolsAsync();
foreach (var tool in mcpTools)
{
    Console.WriteLine($"Connected to server with tools: {tool.Name}");
}


await Task.Delay(100);

// Prompt loop
Console.WriteLine("Type your message below (type 'exit' to quit):");

while (true)
{
    Console.Write("\n Dear Human Says: ");
    //Console.Write("\n You: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
        continue;

    if (userInput.Trim().ToLower() == "exit")
    {
        Console.WriteLine("Exiting chat...");
        break;
    }

    var messages = new List<ChatMessage>
    {
        new(ChatRole.System, "You are a galaxy assistant and help user query about galaxy clients and standards"),
        //new(ChatRole.System, "You are a helpful assistant."),
        new(ChatRole.User, userInput)
    };

    try
    {
        Console.Write("\n AI Response: ");
        //Console.Write("\n AI: ");
        Console.Out.Flush();

        bool hasContent = false;
        await foreach (var update in chatClient.GetStreamingResponseAsync(
            messages,
            new ChatOptions { Tools = [.. mcpTools] }))
        {
            // Display text content as it arrives
            var textContent = update.Contents?.OfType<TextContent>().FirstOrDefault()?.Text;
            if (!string.IsNullOrEmpty(textContent))
            {
                Console.Write(textContent);
                Console.Out.Flush();
                hasContent = true;
            }

        }

        if (!hasContent)
        {
            Console.WriteLine("(no response received)");
        }
        else
        {
            Console.WriteLine(); // Final newline
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n Error: {ex.Message}");
    }
}


// Real-time progress logging middleware
public class RealTimeProgressChatClient : DelegatingChatClient
{
    public RealTimeProgressChatClient(IChatClient innerClient) : base(innerClient) { }

    public override async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> chatMessages,
        ChatOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"\n>>> [{DateTime.Now:HH:mm:ss}] AI is processing your request...");
        Console.Out.Flush();

        bool firstToken = false;
        await foreach (var update in base.GetStreamingResponseAsync(chatMessages, options, cancellationToken))
        {
            // Log when first text token arrives
            if (!firstToken && update.Contents?.OfType<TextContent>().Any() == true)
            {
                Console.WriteLine($">>> [{DateTime.Now:HH:mm:ss}] Streaming response...\n");
                Console.Out.Flush();
                firstToken = true;
            }

            // Log tool calls in real-time
            foreach (var call in update.Contents?.OfType<FunctionCallContent>() ?? Enumerable.Empty<FunctionCallContent>())
            {
                Console.WriteLine($"\n>>> [{DateTime.Now:HH:mm:ss}] Calling tool: {call.Name}");
                if (call.Arguments != null)
                {
                    Console.WriteLine($"    Arguments: {JsonSerializer.Serialize(call.Arguments)}");
                }
                Console.Out.Flush();
            }

            // Log tool results in real-time
            foreach (var result in update.Contents?.OfType<FunctionResultContent>() ?? Enumerable.Empty<FunctionResultContent>())
            {
                Console.WriteLine($">>> [{DateTime.Now:HH:mm:ss}] Tool '{result.Result}' completed");
                Console.Out.Flush();
            }

            yield return update;
        }

        Console.WriteLine($"\n>>> [{DateTime.Now:HH:mm:ss}] Complete!");
        Console.Out.Flush();
    }
}
