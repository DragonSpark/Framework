using DragonSpark.Compose;
using ElasticEmailClient;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace DragonSpark.ElasticEmail
{
	sealed class EmailSender : IEmailSender
	{
		readonly string _name;
		readonly string _address;

		public EmailSender(ElasticEmailSettings settings) : this(settings.FromName, settings.FromAddress) {}

		public EmailSender(string name, string address)
		{
			_name    = name;
			_address = address;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
			=> Api.Email.SendAsync(subject, _address, _name, msgTo: email.Yield(), bodyHtml: htmlMessage);
	}
}