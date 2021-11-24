using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IAccessToken>()
		         .Forward<GetAccessToken>()
		         .Decorate<MemoryAwareAccessToken>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<IUserIdentifierQuery>()
		         .Forward<UserIdentifierQuery>()
		         .Include(x => x.Dependencies)
		         .Singleton()
			;
	}
}