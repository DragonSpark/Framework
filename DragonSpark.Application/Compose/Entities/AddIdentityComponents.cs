using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class AddIdentityComponents<T> : ICommand<IServiceCollection> where T : class
	{
		public static AddIdentityComponents<T> Default { get; } = new AddIdentityComponents<T>();

		AddIdentityComponents() {}

		public void Execute(IServiceCollection parameter)
		{
			var stamp = new ConfigureSecurityStamp(parameter.Deferred<IKnownClaims>());
			parameter.Start<IStateViews<T>>()
			         .Forward<StateViews<T>>()
			         .Decorate<MemoryAwareStateViews<T>>()
			         .Decorate<AnonymousAwareState<T>>()
			         .Scoped()
			         //
			         .Then.Start<IAdapters>()
			         .Forward<Adapters<T>>()
			         .Scoped()
			         //
			         .Then.Start<IAuthenticationValidation>()
			         .Forward<AuthenticationValidation<T>>()
			         .Scoped()
			         .Then.Start<IValidationServices>()
			         .Forward<ValidationServices>()
			         .Scoped()
			         .Then.Start<AuthenticationStateProvider>()
			         .Forward<Revalidation>()
			         .Scoped()
			         //
			         .Then.AddScoped<IUserClaimsPrincipalFactory<T>, UserClaimsPrincipals<T>>()
			         //
			         .Decorate<INavigateToSignOut, AuthenticationStateAwareNavigateToSignOut>()
			         //
			         .Configure<SecurityStampValidatorOptions>(stamp.Execute);
		}
	}
}