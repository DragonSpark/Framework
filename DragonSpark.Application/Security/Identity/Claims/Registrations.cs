using DragonSpark.Application.Security.Identity.Authentication.Persist;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Claims;

public sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IClaims>()
		         .Forward<Compile.Claims>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         .Then.Start<ICurrentKnownClaims>()
		         .Forward<CurrentKnownClaims>()
		         .Scoped()
		         .Then.Start<IDisplayNameClaim>()
		         .Forward<DisplayNameClaim>()
		         .Singleton()
		         //
		         .Then.Start<IExtractClaims>()
		         .Forward<ExtractClaims>()
		         .Singleton()
		         .Then.AddSingleton<IKnownClaims>(KnownClaims.Default)
		         //
		         .Start<IPersistClaims<T>>()
		         .Forward<PersistClaims<T>>()
		         .Decorate<LogAwarePersistClaims<T>>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
				 //
		         //
		         .Then.Start<IPersistSignIn<T>>()
		         .Forward<PersistSignIn<T>>()
		         .Decorate<ClearAwarePersistSignIn<T>>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.Start<IPersistSignInWithMetadata<T>>()
		         .Forward<PersistSignInWithMetadata<T>>()
		         .Decorate<ClearAwarePersistSignInWithMetadata<T>>()
		         .Include(x => x.Dependencies)
		         .Singleton()
			;
	}
}