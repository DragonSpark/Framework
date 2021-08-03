using DragonSpark.Application.Security.Identity.Model.Authenticators;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IAuthenticationRequest>()
			         .Forward<AuthenticationRequest>()
			         .Scoped()
			         //
			         .Then.Start<ExternalLoginChallengingModelBinder>()
			         .And<ChallengedModelBinder>()
			         .And<ReturnOrRoot>()
			         .And<ExternalLoginModel<T>>()
			         .And<ClearCurrentAuthenticationState>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         //
			         .Then.Start<IAddExternalSignin>()
			         .Forward<AddExternalSignin<T>>()
			         .Decorate<StateViewAwareAddExternalSignin>()
			         .Decorate<SignOutAwareAddExternalSignin>()
			         .Decorate<ExceptionAwareAddExternalLogin>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         //
			         .Then.Start<IChallenged<T>>()
			         .Forward<Challenged<T>>()
			         .Decorate<AuthenticationAwareChallenged<T>>()
			         .Scoped()
			         //
			         .Then.Start<IAddLogin<T>>()
			         .Forward<AddLogin<T>>()
			         .Scoped()
			         //
			         .Then.Start<IClearAuthenticationState>()
			         .Forward<ClearAuthenticationState>()
			         .Singleton()
				;
		}
	}
}