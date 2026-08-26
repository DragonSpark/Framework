using System.Net.Http.Json;
using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Model.Operations;

namespace DragonSpark.Grok.Chat;

sealed class ChatResponse : IChatResponse
{
    readonly Func<HttpClient>      _client;
    readonly JsonSerializerOptions _options;
    readonly ToolChoice            _tool;

    public ChatResponse(IHttpClientFactory factory)
        : this(Start.A.Selection<string, HttpClient>(factory.CreateClient).Then().Bind(RegistrationName.Default.Get),
               ApiOptions.Default, ToolChoice.Required) {}

    [Candidate(false)]
    public ChatResponse(Func<HttpClient> client, JsonSerializerOptions options, ToolChoice tool)
    {
        _client  = client;
        _options = options;
        _tool    = tool;
    }

    public async Task<ChatMessage> Get(Stop<ChatResponseInput> parameter)
    {
        var (((name, messages, maximumTokens, temperature), tools), stop) = parameter;

        using var client = _client();
        var payload = new ChatCompletionApiRequest(name, messages, maximumTokens, temperature, tools.Open(), _tool);
        var post = await client.PostAsJsonAsync("chat/completions", payload, _options, stop).Off();
        post.EnsureSuccessStatusCode();
        var response = await post.Content.ReadFromJsonAsync<GrokChatResponse>(_options, stop).Off();
        return response.Verify().Choices[0].Message;
    }
}