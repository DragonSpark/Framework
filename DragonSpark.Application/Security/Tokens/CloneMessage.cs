using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Security.Tokens;

sealed class CloneMessage : IAltering<HttpRequestMessage>
{
    public static CloneMessage Default { get; } = new();

    CloneMessage() : this(ProofName.Default) {}

    readonly string _name;

    public CloneMessage(string name) => _name = name;

    public async ValueTask<HttpRequestMessage> Get(Stop<HttpRequestMessage> parameter)
    {
        var (message, stop) = parameter;

        var result = new HttpRequestMessage(message.Method, message.RequestUri);

        foreach (var h in message.Headers)
        {
            if (!string.Equals(h.Key, _name, StringComparison.OrdinalIgnoreCase))
            {
                result.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
        }

        if (message.Content is not null)
        {
            var ms = new MemoryStream();
            await message.Content.CopyToAsync(ms, stop).Off();
            ms.Position    = 0;
            result.Content = new StreamContent(ms);
            foreach (var h in message.Content.Headers)
            {
                result.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }
        }

        return result;
    }
}