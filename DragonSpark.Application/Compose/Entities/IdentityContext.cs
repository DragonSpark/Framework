using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityContext
	{
		readonly ApplicationProfileContext _context;
		readonly Action<IdentityOptions>   _configure;

		public IdentityContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public IdentityClaimsContext<T> CreatedWith<T>(ISelect<ExternalLoginInfo, T> create) where T : IdentityUser
			=> CreatedWith(create.Get);

		public IdentityClaimsContext<T> CreatedWith<T>(Func<ExternalLoginInfo, T> create) where T : IdentityUser
			=> new IdentityClaimsContext<T>(_context, _configure, create);
	}

	public sealed class IdentityClaimsContext<T> where T : IdentityUser
	{
		readonly ApplicationProfileContext  _context;
		readonly Action<IdentityOptions>    _configure;
		readonly Func<ExternalLoginInfo, T> _create;

		public IdentityClaimsContext(ApplicationProfileContext context, Action<IdentityOptions> configure,
		                             Func<ExternalLoginInfo, T> create)
		{
			_context   = context;
			_configure = configure;
			_create    = create;
		}

		public ConfiguredIdentityContext<T> Having(IClaims claims)
			=> new ConfiguredIdentityContext<T>(_context.Then(new IdentityRegistration<T>(claims, _create)),
			                                    _configure);
	}

	public sealed class ConfiguredIdentityContext<T> where T : IdentityUser
	{
		readonly ApplicationProfileContext _context;
		readonly Action<IdentityOptions>   _configure;

		public ConfiguredIdentityContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public EntityStorageContext<TContext, T> StoredIn<TContext>() where TContext : IdentityDbContext<T>
			=> new EntityStorageContext<TContext, T>(_context, _configure);
	}
}