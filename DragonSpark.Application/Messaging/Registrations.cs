using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Messaging;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<EmailMessagingSettings>()
		         .Start<IAllowSend>()
		         .Forward<AllowSend>()
		         .Singleton()
				 //
				 .Then.TryDecorate<IEmailSender, EmailSender>();
	}
}