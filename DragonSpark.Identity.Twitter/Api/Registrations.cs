using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;

namespace DragonSpark.Identity.Twitter.Api;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<TwitterApiSettings>()
		         //
		         .Start<TwitterClient>()
		         .Use<TwitterClients>()
		         .Singleton()
		         //
		         .Then.Start<ITwitterIdentity>()
		         .Forward<TwitterIdentity>()
		         .Decorate<ValidationAwareTwitterIdentity>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton();
	}
}