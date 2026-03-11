using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.General;
using DragonSpark.Model.Operations;

namespace DragonSpark.Grok.Chat;

public sealed class ChatResult : IChatResult
{
    readonly IHttpClientFactory _factory;
    readonly string             _name;

    public ChatResult(IHttpClientFactory factory) : this(factory, RegistrationName.Default) {}

    public ChatResult(IHttpClientFactory factory, string name)
    {
        _factory = factory;
        _name    = name;
    }

    public async Task<ChatMessage> Get(Stop<ChatModelInput> parameter)
    {
        var ((messages, name, maximumTokens, temperature), stop) = parameter;

        using var client  = _factory.CreateClient(_name);
        var       payload = new ChatCompletionRequest(name, messages, maximumTokens, temperature);
        var       post    = await client.PostAsJsonAsync("chat/completions", payload, stop).Off();
        post.EnsureSuccessStatusCode();
        
        var response = await post.Content.ReadFromJsonAsync<GrokChatResponse>(stop).Off();
        return new("assistant", response.Verify().Choices[0].Message.Content);
    }
}