using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Tweetinvi;

namespace DragonSpark.Identity.Twitter
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<TwitterClient>()
			         .Use<TwitterClients>()
			         .Singleton()
			         //
			         .Then.Start<ITwitterIdentity>()
			         .Forward<TwitterIdentity>()
			         .Include(x => x.Dependencies.Recursive())
			         .Singleton();
		}
	}
}