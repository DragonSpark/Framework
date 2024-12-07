using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace DragonSpark.Application.Messaging;

sealed class EmailSender : IEmailSender
{
	readonly IEmailSender _previous;
	readonly IAllowSend   _allow;

	public EmailSender(IEmailSender previous, IAllowSend allow)
	{
		_previous = previous;
		_allow    = allow;
	}

	public Task SendEmailAsync(string email, string subject, string htmlMessage)
		=> _allow.Get(new(email, subject, htmlMessage))
			   ? _previous.SendEmailAsync(email, subject, htmlMessage)
			   : Task.CompletedTask;
}