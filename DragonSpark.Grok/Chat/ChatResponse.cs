using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Model.Operations;

namespace DragonSpark.Grok.Chat;

public sealed class ChatResponse : IChatResponse
{
    readonly IHttpClientFactory    _factory;
    readonly string                _name;
    readonly JsonSerializerOptions _options;

    public ChatResponse(IHttpClientFactory factory) : this(factory, RegistrationName.Default, ChatOptions.Default) {}

    public ChatResponse(IHttpClientFactory factory, string name, JsonSerializerOptions options)
    {
        _factory = factory;
        _name    = name;
        _options = options;
    }

    public async Task<ChatMessage> Get(Stop<ChatResponseInput> parameter)
    {
        var (((name, messages, maximumTokens, temperature), tools), stop) = parameter;

        using var client  = _factory.CreateClient(_name);
        var       payload = new ChatCompletionApiRequest(name, messages, maximumTokens, temperature, tools.Open());
        var       post    = await client.PostAsJsonAsync("chat/completions", payload, _options, stop).Off();
        post.EnsureSuccessStatusCode();
        var response = await post.Content.ReadFromJsonAsync<GrokChatResponse>(_options, stop).Off();
        return response.Verify().Choices[0].Message;
    }
}