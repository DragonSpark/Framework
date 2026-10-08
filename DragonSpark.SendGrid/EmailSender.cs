using DragonSpark.Composition;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DragonSpark.SendGrid;

sealed class EmailSender : IEmailSender
{
	readonly ISendGridClient _client;
	readonly EmailAddress    _from;
	readonly EmailAddress?   _replyTo;

	public EmailSender(ISendGridClient client, SendGridSettings settings)
		: this(client, new(settings.FromAddress, settings.FromName), settings.ReplyTo) {}

	[Candidate(false)]
	public EmailSender(ISendGridClient client, EmailAddress from, EmailAddress? replyTo)
	{
		_client  = client;
		_from    = from;
		_replyTo = replyTo;
	}

	public Task SendEmailAsync(string email, string subject, string htmlMessage)
	{
		var to      = new EmailAddress(email);
		var message = MailHelper.CreateSingleEmail(_from, to, subject, null, htmlMessage);
		if (_replyTo is not null)
		{
			message.SetReplyTo(_replyTo);
		}

		var result = _client.SendEmailAsync(message);
		return result;
	}
}