using Microsoft.Agents.AI;
using OpenAI;
using System.ClientModel;

namespace BlazorApp2.Services;

public class AiService
{
    private readonly ChatClientAgent _agent;

    public AiService(IConfiguration config)
    {
        string endpoint = config["OpenRouter:Endpoint"] ?? "https://openrouter.ai/api/v1";
        string apiKey = config["OpenRouter:ApiKey"] ?? throw new Exception("Missing OpenRouter:ApiKey");
        string model = config["OpenRouter:Model"] ?? "openai/gpt-oss-20b:free";

        // ✅ Correct constructor for your OpenAI package version:
        // OpenAIClient(ApiKeyCredential, OpenAIClientOptions)
        var client = new OpenAIClient(
            new ApiKeyCredential(apiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(endpoint)   // ✅ use Endpoint (NOT BaseUri)
            });

        // ✅ Keep Microsoft Agent Framework
        _agent = client.GetChatClient(model).CreateAIAgent();
    }

    public async Task<string> AskAsync(string prompt)
    {
        AgentRunResponse response = await _agent.RunAsync(prompt);
        return response.ToString();
    }
}
