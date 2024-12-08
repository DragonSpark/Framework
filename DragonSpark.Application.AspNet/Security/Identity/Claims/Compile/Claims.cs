using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

sealed class Claims : IClaims
{
	readonly DisplayNameValue                  _display;
	readonly ISelect<ProviderIdentity, string> _formatter;

	public Claims(DisplayNameValue display) : this(display, IdentityFormatter.Default) {}

	public Claims(DisplayNameValue display, ISelect<ProviderIdentity, string> formatter)
	{
		_display   = display;
		_formatter = formatter;
	}

	public IEnumerable<Claim> Get(Login parameter)
	{
		var (principal, provider, key) = parameter;

		yield return new(ExternalIdentity.Default, _formatter.Get(new(provider, key)));
		yield return new(ClaimTypes.AuthenticationMethod, provider);
		yield return new(DisplayName.Default, _display.Get(principal, provider));
	}
}