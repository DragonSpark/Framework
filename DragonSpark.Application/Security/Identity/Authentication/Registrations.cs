using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IAuthenticate<T>>()
		         .Forward<Authenticate<T>>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         .Then.Start<IAuthentication>()
		         .Forward<Authentication>()
		         .Scoped()
		         .Then.Start<IAuthenticationProfile>()
		         .Forward<AuthenticationProfile<T>>()
		         .Singleton()
		         .Then.Start<IExternalSignin>()
		         .Forward<ExternalSignin<T>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<ISignOut>()
		         .Forward<SignOut<T>>()
		         .Singleton()
		         //
		         .Then.Start<IRefreshAuthentication<T>>()
		         .Forward<RefreshAuthentication<T>>()
		         .Include(x => x.Dependencies)
		         .Scoped();
	}
}