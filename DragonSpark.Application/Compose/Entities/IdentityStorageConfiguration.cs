using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityStorageConfiguration<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;
		readonly Action<IdentityOptions>   _configure;

		public IdentityStorageConfiguration(ApplicationProfileContext subject, Action<IdentityOptions> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public ConfiguredIdentityStorage<T, TContext> SqlServer(string name)
			=> Configuration(new SqlStorageConfiguration<TContext>(name));

		public ConfiguredIdentityStorage<T, TContext> SqlServer()
			=> Configuration(SqlStorageConfiguration<TContext>.Default);

		public ConfiguredIdentityStorage<T, TContext> Configuration(IStorageConfiguration configuration)
			=> new(_subject, _configure, configuration);
	}
}