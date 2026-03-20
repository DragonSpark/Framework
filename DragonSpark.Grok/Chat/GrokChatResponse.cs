using System.Collections.Generic;
using DragonSpark.Contracts.General.Chat;

namespace DragonSpark.Grok.Chat;

public sealed record GrokChatResponse(
    string Id,
    string Object,
    long Created,
    string Model,
    List<ChatChoice> Choices,
    Usage Usage);