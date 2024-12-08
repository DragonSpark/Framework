using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.AspNet.Security.Identity.Authentication.State;
using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet.Compose.Entities;

sealed class AddIdentityComponents<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static AddIdentityComponents<T> Default { get; } = new();

	AddIdentityComponents() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IStateUser<T>>()
		         .Forward<StateUser<T>>()
		         .Decorate<PolicyAwareStateUser<T>>()
		         .Decorate<MemoryAwareStateUser<T>>()
		         .Singleton()
		         //
		         .Then.Start<IStateViews<T>>()
		         .Forward<StateViews<T>>()
		         .Decorate<MemoryAwareStateViews<T>>()
		         .Decorate<AnonymousAwareState<T>>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.Start<CurrentProfileStatus>()
		         .And<RefreshAuthenticationDisplayState<T>>()
		         .And<AuthenticationStore>()
		         .Scoped()
		         //
		         .Then.Start<IAdapters>()
		         .Forward<Adapters<T>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IAuthenticationValidation>()
		         .Forward<AuthenticationValidation<T>>()
		         .Singleton()
		         .Then.Start<IValidationServices>()
		         .Forward<ValidationServices>()
		         .Scoped()
		         .Then.Start<AuthenticationStateProvider>()
		         .Forward<Revalidation>()
		         .Scoped()
		         //
		         .Then.Start<IUserClaimsPrincipalFactory<T>>()
		         .Forward<UserClaimsPrincipals<T>>()
		         .Scoped();
	}
}