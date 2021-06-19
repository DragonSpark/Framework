using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims
{
	sealed class Claims : IClaims
	{
		readonly IDisplayNameClaim                 _display;
		readonly ISelect<ProviderIdentity, string> _formatter;

		public Claims(IDisplayNameClaim display) : this(display, IdentityFormatter.Default) {}

		public Claims(IDisplayNameClaim display, ISelect<ProviderIdentity, string> formatter)
		{
			_display   = display;
			_formatter = formatter;
		}

		public IEnumerable<Claim> Get(Login parameter)
		{
			var (principal, provider, key) = parameter;

			yield return new Claim(ExternalIdentity.Default, _formatter.Get(new(provider, key)));
			yield return new Claim(ClaimTypes.AuthenticationMethod, provider);
			yield return new Claim(DisplayName.Default, principal.FindFirstValue(_display.Get(provider)));
		}
	}
}