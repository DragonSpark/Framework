using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.ElasticEmail
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Register<ElasticEmailSettings>()
			         .Start<Initializer>()
			         .Singleton()
			         //
			         .Then.Start<IEmailSender>()
			         .Forward<EmailSender>()
			         .Singleton();
		}
	}
}