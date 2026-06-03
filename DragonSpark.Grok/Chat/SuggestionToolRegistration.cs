namespace DragonSpark.Grok.Chat;

sealed class SuggestionToolRegistration : ToolRegistration<SuggestionsResult>
{
    public static SuggestionToolRegistration Default { get; } = new();

    SuggestionToolRegistration() : base(SuggestionToolName.Default, Suggestions.Default) {}
}