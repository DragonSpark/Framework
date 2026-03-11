using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using DragonSpark.Contracts.General;

namespace DragonSpark.Grok.Chat;

[method: JsonConstructor]
public sealed record ChatModelInput(
    IReadOnlyCollection<ChatMessage> Messages,
    string Name,
    ushort MaximumTokens,
    double Temperature)
{
    public ChatModelInput(IReadOnlyCollection<ChatMessage> messages, string context)
        : this(messages, context, DefaultModelName.Default) {}

    public ChatModelInput(IReadOnlyCollection<ChatMessage> messages, string context, string name)
        : this(messages, context, name, MaximumTokenCount.Default) {}

    // ReSharper disable once TooManyDependencies
    public ChatModelInput(IReadOnlyCollection<ChatMessage> messages, string context, string name,
                          ushort maximumTokens)
        : this(messages, context, name, maximumTokens, DefaultTemperature.Default) {}

    // ReSharper disable once TooManyDependencies
    public ChatModelInput(IReadOnlyCollection<ChatMessage> messages, string context,
                          string name, ushort maximumTokens, double temperature)
        : this(messages.Prepend(new ChatMessage("system", context)).ToArray(), name, maximumTokens, temperature) {}
}