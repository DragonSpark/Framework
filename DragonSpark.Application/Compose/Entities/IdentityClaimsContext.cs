using DragonSpark.Application.Security.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities
{
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
}