using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities
{
	public sealed class EntityStorageConfigurationContext<T, TUser>
		where T : IdentityDbContext<TUser>
		where TUser : IdentityUser
	{
		readonly IServiceCollection      _collection;
		readonly Action<IdentityOptions> _configure;

		public EntityStorageConfigurationContext(IServiceCollection collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure  = configure;
		}

		public IServiceCollection SqlServer() => Configuration(SqlStorageConfiguration<T>.Default);

		public IServiceCollection Configuration(IStorageConfiguration configuration)
			=> new ConfigureIdentityStorage<T, TUser>(configuration, _configure).Parameter(_collection);
	}
}