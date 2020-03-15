using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Profile;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Compose.Entities {
	public sealed class IdentityClaimsContext<T> where T : IdentityUser
	{
		readonly IdentityClaimsContextParameter<T> _context;
		readonly IAppliedPrincipal                 _principal;

		public IdentityClaimsContext(IdentityClaimsContextParameter<T> context)
			: this(context, DefaultAppliedPrincipal.Default) {}

		public IdentityClaimsContext(IdentityClaimsContextParameter<T> context, IAppliedPrincipal principal)
		{
			_context   = context;
			_principal = principal;
		}

		public IdentityClaimsContext<T> Using(IAppliedPrincipal principal)
			=> new IdentityClaimsContext<T>(_context, principal);

		public ConfiguredIdentityContext<T> Having(IClaims claims)
			=> new ConfiguredIdentityContext<T>(_context.Context.Then(new IdentityRegistration<T>(_principal, claims,
			                                                                                      _context.Create)),
			                                    _context.Configure);
	}
}