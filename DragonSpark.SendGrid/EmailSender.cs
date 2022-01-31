using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DragonSpark.SendGrid;

sealed class EmailSender : IEmailSender
{
	readonly ISendGridClient _client;
	readonly EmailAddress    _from;

	public EmailSender(ISendGridClient client, SendGridSettings settings)
		: this(client, new EmailAddress(settings.FromAddress, settings.FromName)) {}

	public EmailSender(ISendGridClient client, EmailAddress from)
	{
		_client  = client;
		_from    = from;
	}

	public Task SendEmailAsync(string email, string subject, string htmlMessage)
	{
		var to      = new EmailAddress(email);
		var message = MailHelper.CreateSingleEmail(_from, to, subject, null, htmlMessage);
		var result  = _client.SendEmailAsync(message);
		return result;
	}
}