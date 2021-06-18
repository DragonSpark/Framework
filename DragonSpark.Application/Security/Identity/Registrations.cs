using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			// Performance:
			parameter.Scan(x => x.FromAssemblyOf<Registrations<T>>()
			                     .AddClasses(y => y.InExactNamespaces(typeof(Registrations<T>).Namespace.Verify()))
			                     .AsSelf()
			                     .AsMatchingInterface()
			                     .WithScopedLifetime())
			         //
			         .AddScoped<IUserSynchronization, UserSynchronization<T>>()
			         //
			         .AddScoped<ICreateRequest, CreateRequest<T>>()
			         //
			         .Start<ICreate<T>>()
			         .Forward<Create<T>>()
			         .Decorate<LoggingAwareCreate<T>>()
			         .Scoped()
			         .Then.Start<ICreated<T>>()
			         .Forward<Created<T>>()
			         .Decorate<AddLoginAwareCreated<T>>()
			         .Decorate<SynchronizationAwareCreated<T>>()
			         .Scoped()
			         // Model:
			         .Then.Start<IAuthenticate<T>>()
			         .Forward<Authenticate<T>>()
			         .Scoped()
			         .Then.Start<IAuthentication>()
			         .Forward<Model.Authentication>()
			         .Scoped()
			         .Then.Start<IAuthenticationProfile>()
			         .Forward<AuthenticationProfile<T>>()
			         .Scoped()
			         .Then.Start<IAuthenticationRequest>()
			         .Forward<AuthenticationRequest>()
			         .Scoped()
			         .Then.Start<IClaims>()
			         .Forward<Claims>()
			         .Scoped()
			         .Then.Start<ICurrentKnownClaims>()
			         .Forward<CurrentKnownClaims>()
			         .Scoped()
			         .Then.Start<IDisplayNameClaim>()
			         .Forward<DisplayNameClaim>()
			         .Scoped()
			         .Then.Start<IExternalSignin>()
			         .Forward<ExternalSignin<T>>()
			         .Scoped()
			         .Then.Start<IExtractClaims>()
			         .Forward<ExtractClaims>()
			         .Scoped()
			         .Then.Start<IKnownClaims>()
			         .Forward<KnownClaims>()
			         .Scoped()
			         //
			         .Then.Start<ExternalLoginChallengingModelBinder>()
			         .And<ChallengedModelBinder>()
			         .And<ReturnOrRoot>()
			         .And<ExternalLoginModel<T>>()
			         .Include(x => x.Dependencies)
			         .Scoped()
				//
				;
		}
	}
}