﻿using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities;

sealed class AddIdentityComponents<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static AddIdentityComponents<T> Default { get; } = new AddIdentityComponents<T>();

	AddIdentityComponents() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IStateViews<T>>()
		         .Forward<StateViews<T>>()
		         .Decorate<PolicyAwareStateViews<T>>()
		         .Decorate<MemoryAwareStateViews<T>>()
		         .Decorate<AnonymousAwareState<T>>()
		         .Singleton()
		         //
		         .Then.Start<CurrentProfileStatus>()
		         .And<RefreshAuthenticationDisplayState<T>>()
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