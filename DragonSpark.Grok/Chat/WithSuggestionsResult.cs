using System.Collections.Immutable;
using DragonSpark.Contracts.General.Chat;

namespace DragonSpark.Grok.Chat;

public readonly record struct WithSuggestionsResult(
    ImmutableArray<ChatMessage> History,
    ImmutableArray<string>? Suggestions);