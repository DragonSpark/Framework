using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityContext<T> where T : IdentityUser
	{
		readonly ApplicationProfileContext _collection;
		readonly Action<IdentityOptions> _configure;

		public IdentityContext(ApplicationProfileContext collection, Action<IdentityOptions> configure)
		{
			_collection = collection;
			_configure  = configure;
		}

		public EntityStorageContext<TContext, T> StoredIn<TContext>() where TContext : IdentityDbContext<T>
			=> new EntityStorageContext<TContext, T>(_collection, _configure);
	}
}