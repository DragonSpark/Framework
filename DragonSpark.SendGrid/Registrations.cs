using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace DragonSpark.SendGrid;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new ();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		var register = new Register(parameter.Deferred<SendGridSettings>());
		parameter.Register<SendGridSettings>()
		         .AddSendGrid(register.Execute)
		         .Services.Start<IEmailSender>()
		         .Forward<EmailSender>()
		         .Singleton();
	}
}