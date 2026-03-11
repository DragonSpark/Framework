namespace DragonSpark.Grok.Chat;

public sealed class RegistrationName : Text.Text
{
    public static RegistrationName Default { get; } = new();

    RegistrationName() : base("GrokApi") {}
}