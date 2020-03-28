﻿using DragonSpark.Application.Entities;
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
			         .AddScoped<AuthenticationStateProvider, Revalidation<TUser>>()
			         .AddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipals<TUser>>();
		}
	}
}