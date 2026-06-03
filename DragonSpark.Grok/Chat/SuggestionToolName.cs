namespace DragonSpark.Grok.Chat;

public sealed class SuggestionToolName : Text.Text
{
    public static SuggestionToolName Default { get; } = new();

    SuggestionToolName() : base(nameof(SuggestionToolRegistration)) {}
}