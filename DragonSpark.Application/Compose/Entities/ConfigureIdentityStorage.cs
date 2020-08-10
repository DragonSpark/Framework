using DragonSpark.Application.Entities;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureIdentityStorage<T, TUser> : ICommand<IServiceCollection>
		where T : DbContext where TUser : class
	{
		readonly Action<IdentityOptions> _identity;
		readonly IStorageConfiguration   _storage;

		public ConfigureIdentityStorage(IStorageConfiguration storage, Action<IdentityOptions> identity)
		{
			_storage  = storage;
			_identity = identity;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContext<T>(_storage.Get(parameter))
			         .AddDefaultIdentity<TUser>(_identity)
			         .AddEntityFrameworkStores<T>()
			         .Return(parameter)
			         .Start<IStorageInitializer<T>>()
			         .Forward<StorageInitializer<T>>()
			         .Singleton()
			         .Then.AddScoped<DbContext>(x => x.GetRequiredService<T>())
			         //
			         .Start<IStateViews<TUser>>()
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