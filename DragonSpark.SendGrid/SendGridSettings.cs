using SendGrid.Helpers.Mail;

namespace DragonSpark.SendGrid;

public sealed record SendGridSettings
{
	public required string FromAddress { get; set; }

	public required string FromName { get; set; }

	public EmailAddress? ReplyTo { get; set; }

	public required string ApiKey { get; set; }
}