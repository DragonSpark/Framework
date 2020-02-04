using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Server.Entities {
	public sealed class EntityConfigurationContext<T> where T : DbContext
	{
		readonly IServiceCollection _collection;

		public EntityConfigurationContext(IServiceCollection collection) => _collection = collection;

		public IServiceCollection And<TUser>() where TUser : class => And<TUser>(_ => {});

		public IServiceCollection And<TUser>(Action<IdentityOptions> configure) where TUser : class
			=> new ConfigureEntityStorage<T, TUser>(configure).Parameter(_collection);
	}
}