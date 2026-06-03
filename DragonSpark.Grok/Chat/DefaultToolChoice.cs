using DragonSpark.Model.Results;

namespace DragonSpark.Grok.Chat;

sealed class DefaultToolChoice : Instance<ToolChoice>
{
    public static DefaultToolChoice Default { get; } = new();

    DefaultToolChoice() : base(ToolChoice.For(SuggestionToolName.Default)) {}
}