namespace DragonSpark.Grok.Chat;

public sealed class DefaultModelName : Text.Text
{
    public static DefaultModelName Default { get; } = new();

    DefaultModelName() : base("grok-4-1-fast-reasoning") {}
}