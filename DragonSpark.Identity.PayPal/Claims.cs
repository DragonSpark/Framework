using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Identity.PayPal;

sealed class Claims : IClaims
{
	readonly IClaims       _previous;
	readonly Array<string> _names;

	public Claims(IClaims previous) : this(previous, KnownClaims.Default) {}

	public Claims(IClaims previous, Array<string> names)
	{
		_previous = previous;
		_names    = names;
	}

	public IEnumerable<Claim> Get(Login parameter)
	{
		var (principal, provider, _) = parameter;
		var previous = _previous.Get(parameter);
		switch (provider)
		{
			case PaypalAuthenticationDefaults.AuthenticationScheme:
			{
				var list = previous.ToList();
				foreach (var name in _names)
				{
					if (principal.HasClaim(name))
					{
						list.Add(new(name, principal.FindFirstValue(name).Verify()));
					}
				}

				return list.AsEnumerable();
			}
			default:
				return previous;
		}
	}
}