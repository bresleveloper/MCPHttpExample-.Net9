

# MCPHttpSExample

## Relevant Links:

[youtute tutorial HEBREW](...)

* [demo tutorial in Medium](https://medium.com/@mutluozkurt/creating-an-mcp-server-and-client-with-net-a-step-by-step-guide-0c3833dde3c4)
* [official C# sdk](https://github.com/modelcontextprotocol/csharp-sdk)
* [run N8N locally](https://docs.n8n.io/integrations/creating-nodes/test/run-node-locally/)


## run N8N locally

* `npm install n8n -g`
* `n8n start`
* browse to `http://localhost:5678/`

for cloud use (google) `hostiner n8n server`



## Dependencies for all projects

**newest**
* `dotnet add package ModelContextProtocol --prerelease`                
* `dotnet add package ModelContextProtocol.AspNetCore --prerelease`     
* `dotnet add package Microsoft.Extensions.Hosting`

**version specific for this project**
* `dotnet add package ModelContextProtocol --version 0.4.0-preview.3`                
* `dotnet add package ModelContextProtocol.AspNetCore --version 0.4.0-preview.3`     
* `dotnet add package Microsoft.Extensions.Hosting`


## Create SLN

* `dotnet new sln --name MCPHttpExample`


## Create MCPHttpServerExample

* Create Folder
* `dotnet new web`
* `dotnet sln add MCPHttpServerExample` (folder name)
* dependencies...

## Create MCPHttpClientExample (for Ollama)

* Create Folder
* `dotnet new console`
* `dotnet sln add MCPHttpClientExample` (folder name)
* dependencies...
* `dotnet add package OllamaSharp`
* `dotnet add package Microsoft.Extensions.AI`



## IF OFFLINE (restore not working)

`dotnet run --no-restore`


## prompts:

* `get clients`
* `get clients names by galaxy standard`
* `i need client with id 4 name in galaxy standard`


## Cloud Deploy (Server)


* `dotnet publish -c Release -o ./publish`
* `scp -r ./publish/* appsrunner@185.220.206.118:/var/www/MCPHttpServerExample/`
* `systemctl restart MCPHttpServerExample`
* test localhost n8n with `http://0.0.0.0:4006`




## Debian / Linux

in case you want to have some linux fun time, my journey is here

`https://gist.github.com/bresleveloper/4d43a1716827a1fe7f6d50903c9e137d`



