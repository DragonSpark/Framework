namespace DragonSpark.SendGrid;

public sealed record SendGridSettings
{
	public string FromAddress { get; set; } = null!;

	public string FromName { get; set; } = null!;

	public string ApiKey { get; set; } = null!;
}