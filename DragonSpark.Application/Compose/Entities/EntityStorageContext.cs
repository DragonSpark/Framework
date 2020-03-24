using Microsoft.AspNetCore.Identity;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class EntityStorageContext<T, TUser> where T : Security.Identity.IdentityDbContext<TUser> 
	                                                   where TUser : IdentityUser
	{
		readonly ApplicationProfileContext _collection;
		readonly Action<IdentityOptions> _configure;

		public EntityStorageContext(ApplicationProfileContext collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure  = configure;
		}

		public EntityStorageConfigurationContext<T, TUser> Using
			=> new EntityStorageConfigurationContext<T, TUser>(_collection, _configure);
	}
}