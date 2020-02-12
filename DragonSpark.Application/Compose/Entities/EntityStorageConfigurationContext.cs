using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class EntityStorageConfigurationContext<T, TUser>
		where T : IdentityDbContext<TUser>
		where TUser : IdentityUser
	{
		readonly ServerProfileContext    _context;
		readonly Action<IdentityOptions> _configure;

		public EntityStorageConfigurationContext(ServerProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public ServerProfileContext SqlServer() => Configuration(SqlStorageConfiguration<T>.Default);

		public ServerProfileContext Configuration(IStorageConfiguration configuration)
			=> _context.Then(new ConfigureIdentityStorage<T, TUser>(configuration, _configure));
	}
}