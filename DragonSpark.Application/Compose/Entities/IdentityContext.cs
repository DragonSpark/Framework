using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
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
			=> new IdentityClaimsContext<T>(new IdentityClaimsContextParameter<T>(_context, _configure, create));
	}
}