using DragonSpark.Application.Security.Identity.Authentication.Persist;
using DragonSpark.Application.Security.Identity.Claims.Compile;
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
		         .Singleton()
				 //
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
				 //
		         .Then.Start<IPersistSignIn<T>>()
		         .Forward<PersistSignIn<T>>()
		         .Singleton()
		         //
		         .Then.Start<IPersistSignInWithMetadata<T>>()
		         .Forward<PersistSignInWithMetadata<T>>()
		         .Singleton()
			;
	}
}