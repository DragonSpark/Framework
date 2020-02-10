using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities
{
	/*public sealed class EntityConfigurationContext<T> where T : DbContext
	{
		readonly IServiceCollection _collection;

		public EntityConfigurationContext(IServiceCollection collection) => _collection = collection;

		public IServiceCollection And<TUser>() where TUser : class => And<TUser>(_ => {});

		public IServiceCollection And<TUser>(Action<IdentityOptions> configure) where TUser : class
			=> new ConfigureSqlServer<T, TUser>(configure).Parameter(_collection);
	}*/

	public sealed class IdentityContext<T> where T : IdentityUser
	{
		readonly IServiceCollection _collection;
		readonly Action<IdentityOptions> _configure;

		public IdentityContext(IServiceCollection collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure = configure;
		}

		public EntityStorageContext<TContext, T> StoredIn<TContext>() where TContext : IdentityDbContext<T>
			=> new EntityStorageContext<TContext, T>(_collection, _configure);
	}

	public sealed class EntityStorageContext<T, TUser> where T : IdentityDbContext<TUser> where TUser : IdentityUser
	{
		readonly IServiceCollection _collection;
		readonly Action<IdentityOptions> _configure;

		public EntityStorageContext(IServiceCollection collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure = configure;
		}

		public EntityStorageConfigurationContext<T, TUser> Using
			=> new EntityStorageConfigurationContext<T, TUser>(_collection, _configure);
	}

	public sealed class EntityStorageConfigurationContext<T, TUser> 
		where T : IdentityDbContext<TUser> 
		where TUser : IdentityUser
	{
		readonly IServiceCollection _collection;
		readonly Action<IdentityOptions> _configure;

		public EntityStorageConfigurationContext(IServiceCollection collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure = configure;
		}

		public IServiceCollection SqlServer() => Configuration(SqlStorageConfiguration<T>.Default);

		public IServiceCollection Configuration(IStorageConfiguration configuration)
			=> new ConfigureIdentityStorage<T, TUser>(configuration, _configure).Parameter(_collection);
	}
}