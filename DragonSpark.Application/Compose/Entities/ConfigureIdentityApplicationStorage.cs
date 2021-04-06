using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureIdentityApplicationStorage<T, TUser> : ICommand<IServiceCollection>
		where T : DbContext where TUser : class
	{
		readonly ICommand<IServiceCollection> _previous;

		public ConfigureIdentityApplicationStorage(IStorageConfiguration storage, Func<IServiceProvider, T> factory,
		                                           Action<IdentityOptions> identity)
			: this(new ConfigureStorage<TUser, T>(storage, factory, identity)) {}

		public ConfigureIdentityApplicationStorage(ICommand<IServiceCollection> previous) => _previous = previous;

		public void Execute(IServiceCollection parameter)
		{
			_previous.Execute(parameter);

			parameter.Start<IStateViews<TUser>>()
			         .Forward<StateViews<TUser>>()
			         .Decorate<StoredStateViews<TUser>>()
			         .Decorate<AnonymousAwareState<TUser>>()
			         .Scoped()
			         //
			         .Then.Start<IAdapters>()
			         .Forward<Adapters<TUser>>()
			         .Scoped()
			         //
			         .Then.Start<IAuthenticationValidation>()
			         .Forward<AuthenticationValidation<TUser>>()
			         .Scoped()
			         .Then.Start<IValidationServices>()
			         .Forward<ValidationServices>()
			         .Scoped()
			         .Then.Start<AuthenticationStateProvider>()
			         .Forward<Revalidation>()
			         .Scoped()
			         //
			         .Then.AddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipals<TUser>>()
					 //
			         .Decorate<INavigateToSignOut, MemoryAwareNavigateToSignOut<TUser>>();
		}
	}
}