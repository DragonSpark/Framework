using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityStorageUsing<T, TContext> where TContext : DbContext where T : class
	{
		readonly ApplicationProfileContext _subject;
		readonly Action<IdentityOptions>   _configure;

		public IdentityStorageUsing(ApplicationProfileContext subject, Action<IdentityOptions> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public IdentityStorageConfiguration<T, TContext> Using => new(_subject, _configure);
	}
}