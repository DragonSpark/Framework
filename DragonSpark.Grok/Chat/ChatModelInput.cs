using System.Text.Json.Serialization;
using DragonSpark.Contracts.General.Chat;

namespace DragonSpark.Grok.Chat;

[method: JsonConstructor]
public sealed record ChatModelInput(
    string Name,
    IReadOnlyCollection<ChatMessage> Messages,
    ushort MaximumTokens,
    double Temperature)
{
    public ChatModelInput(IReadOnlyCollection<ChatMessage> messages, string context)
        : this(DefaultModelName.Default, messages, context) {}

    public ChatModelInput(string name, IReadOnlyCollection<ChatMessage> messages, string context)
        : this(name, messages, context, MaximumTokenCount.Default) {}

    // ReSharper disable once TooManyDependencies
    public ChatModelInput(string name, IReadOnlyCollection<ChatMessage> messages, string context,
                          ushort maximumTokens)
        : this(name, messages, context, maximumTokens, DefaultTemperature.Default) {}

    // ReSharper disable once TooManyDependencies
    public ChatModelInput(string name, IReadOnlyCollection<ChatMessage> messages,
                          string context, ushort maximumTokens, double temperature)
        : this(name, messages.Prepend(new SystemMessage(context)).ToArray(), maximumTokens, temperature) {}
}