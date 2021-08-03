using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IAuthenticate<T>>()
			         .Forward<Authenticate<T>>()
			         .Scoped()
			         .Then.Start<IAuthentication>()
			         .Forward<Authentication>()
			         .Scoped()
			         .Then.Start<IAuthenticationProfile>()
			         .Forward<AuthenticationProfile<T>>()
			         .Scoped()
			         .Then.Start<IExternalSignin>()
			         .Forward<ExternalSignin<T>>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         //
			         .Then.Start<IRefreshAuthentication<T>>()
			         .Forward<RefreshAuthentication<T>>()
			         .Include(x => x.Dependencies)
			         .Scoped()

				//
				;
		}
	}
}