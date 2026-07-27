namespace DragonSpark.Grok.Chat;

public sealed record GrokApiSettings
{
    public required string Key { get; set; }
    public required Uri Location { get; set; } = new("https://api.x.ai/v1/");
}