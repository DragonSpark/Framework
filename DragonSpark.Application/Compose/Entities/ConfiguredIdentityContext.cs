using Microsoft.AspNetCore.Identity;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class ConfiguredIdentityContext<T> where T : IdentityUser
	{
		readonly ApplicationProfileContext _context;
		readonly Action<IdentityOptions>   _configure;

		public ConfiguredIdentityContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public EntityStorageContext<TContext, T> StoredIn<TContext>()
			where TContext : Security.Identity.IdentityDbContext<T>
			=> new EntityStorageContext<TContext, T>(_context, _configure);
	}
}