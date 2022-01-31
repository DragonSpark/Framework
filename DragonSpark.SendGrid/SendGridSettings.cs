namespace DragonSpark.SendGrid;

public sealed class SendGridSettings
{
	public string FromAddress { get; set; } = default!;

	public string FromName { get; set; } = default!;

	public string ApiKey { get; set; } = default!;
}