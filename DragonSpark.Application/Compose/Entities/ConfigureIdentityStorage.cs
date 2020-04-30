using DragonSpark.Application.Entities;
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
			         .For<IStorageInitializer<T>>().Map<StorageInitializer<T>>().Singleton()
			         .For<DbContext>().Map<T>().Scoped()
					 //
			         .For<IProfileStates<TUser>>().Map<ProfileStates<TUser>>().Scoped()
			         .Decorate<IProfileStates<TUser>, AnonymousAwareProfile<TUser>>()
			         .Decorate<IProfileStates<TUser>, StoredProfileStates<TUser>>()

					 //
			         .For<IAdapters>().Map<Adapters<TUser>>().Scoped()
					 //
					 .For<IAuthenticationValidation>().Map<AuthenticationValidation<TUser>>().Scoped()
					 .For<IValidationServices>().Map<ValidationServices>().Scoped()
					 .For<AuthenticationStateProvider>().Map<Revalidation>().Scoped()
					 //
			         .AddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipals<TUser>>();
		}
	}
}