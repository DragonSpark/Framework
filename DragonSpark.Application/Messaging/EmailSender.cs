using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace DragonSpark.Application.Messaging;

sealed class EmailSender : IEmailSender
{
	readonly IEmailSender _previous;
	readonly bool         _enabled;

	public EmailSender(IEmailSender previous, EmailMessagingSettings settings) : this(previous, settings.Enabled) {}

	public EmailSender(IEmailSender previous, bool enabled)
	{
		_previous = previous;
		_enabled  = enabled;
	}

	public Task SendEmailAsync(string email, string subject, string htmlMessage)
		=> _enabled ? _previous.SendEmailAsync(email, subject, htmlMessage) : Task.CompletedTask;
}