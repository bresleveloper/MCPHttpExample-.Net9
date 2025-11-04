

using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;
using System.ComponentModel;



var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();



var app = builder.Build();
app.MapMcp();
app.Run("http://0.0.0.0:4007");


[McpServerToolType]
public static class ReverseTool
{
    [McpServerTool]
    [Description("reverse the message by galaxy standards")]
    public static string reverse(string message) => $"GALAXY SAIS {new string(message.Reverse().ToArray())}";
}






