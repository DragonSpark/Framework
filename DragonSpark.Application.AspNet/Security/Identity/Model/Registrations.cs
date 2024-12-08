using DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IAuthenticateRequest>()
		         .Forward<AuthenticateRequest>()
		         .Scoped()
		         //
		         .Then.Start<ChallengedModelBinder>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<ExternalLoginChallengingModelBinder>()
		         .And<ReturnOrRoot>()
		         .And<ExternalLoginModel<T>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IAddExternalSignin>()
		         .Forward<AddExternalSignin<T>>()
		         .Decorate<StateAwareAddExternalSignin>()
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
		         .Singleton()
		         //
		         .Then.Start<IRemoveLogin<T>>()
		         .Forward<RemoveLogin<T>>()
		         .Decorate<StateAwareRemoveLogin<T>>()
		         .Singleton()
		         //
		         .Then.Start<IClearAuthenticationState>()
		         .Forward<ClearAuthenticationState>()
		         .Singleton()
			;
	}
}