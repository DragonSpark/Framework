using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Extensions.DependencyInjection;

namespace DragonSpark.SendGrid;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<SendGridSettings>()
		         .AddSendGrid(_ => {})
		         .Services.AddOptions<SendGridClientOptions>()
		         .Configure<SendGridSettings>((options, settings) => options.ApiKey = settings.ApiKey)
		         .Services.Start<IEmailSender>()
		         .Forward<EmailSender>()
		         .Singleton();
	}
}