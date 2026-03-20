using System.Text.Json;
using System.Text.Json.Serialization;
using DragonSpark.Application.Communication.Http.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Grok.Chat;

public sealed class ChatOptions : Instance<JsonSerializerOptions>
{
    public static ChatOptions Default { get; } = new();

    ChatOptions()
        : base(NewOptions.Default.Get(JsonNamingPolicy.SnakeCaseLower)
                         .With(x =>
                               {
                                   x.Converters.Add(ToolCallConverter.Default);
                                   x.DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull;
                                   x.PropertyNameCaseInsensitive = true;
                               })) {}
}