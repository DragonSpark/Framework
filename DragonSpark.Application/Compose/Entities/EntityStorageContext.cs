using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class EntityStorageContext<T, TUser> where T : Security.Identity.IdentityDbContext<TUser>
	                                                   where TUser : Security.Identity.IdentityUser
	{
		readonly ApplicationProfileContext _collection;
		readonly Action<IdentityOptions>   _configure;

		public EntityStorageContext(ApplicationProfileContext collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure  = configure;
		}

		public EntityStorageConfigurationContext<T, TUser> Using => new(_collection, _configure);
	}

	public sealed class IdentityStorageContext<T> where T : class
	{
		readonly ApplicationProfileContext _subject;

		public IdentityStorageContext(ApplicationProfileContext subject) => _subject = subject;

		public IdentityStorageContext<T, TContext> StoredIn<TContext>() where TContext : DbContext => new(_subject);
	}
}