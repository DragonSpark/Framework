using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class EntityStorageConfigurationContext<T, TUser>
		where T : IdentityDbContext<TUser>
		where TUser : IdentityUser
	{
		readonly ApplicationProfileContext    _context;
		readonly Action<IdentityOptions> _configure;

		public EntityStorageConfigurationContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public ApplicationProfileContext SqlServer() => Configuration(SqlStorageConfiguration<T>.Default);

		public ApplicationProfileContext Configuration(IStorageConfiguration configuration)
			=> _context.Then(new ConfigureIdentityStorage<T, TUser>(configuration, _configure));
	}
}