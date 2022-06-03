using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Identity.PayPal;

sealed class Claims : IClaims
{
	readonly IClaims _previous;
	readonly string  _name;

	public Claims(IClaims previous) : this(previous, PayIdentifier.Default) {}

	public Claims(IClaims previous, string name)
	{
		_previous = previous;
		_name     = name;
	}

	public IEnumerable<Claim> Get(Login parameter)
	{
		var (principal, provider, _) = parameter;
		var previous = _previous.Get(parameter);
		var result = provider == PaypalAuthenticationDefaults.AuthenticationScheme && principal.HasClaim(_name)
			             ? previous.Append(new Claim(_name, principal.FindFirstValue(_name)))
			             : previous;
		return result;
	}
}