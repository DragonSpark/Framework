using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Actions;

public class SubKeyClaimAction : IClaimAction
{
	readonly string _claim, _type;
	readonly SubKey _key;

	protected SubKeyClaimAction(string claim, string key, string element)
		: this(claim, new SubKey(key, element), "string") {}

	protected SubKeyClaimAction(string claim, SubKey key, string type)
	{
		_key   = key;
		_claim = claim;
		_type  = type;
	}

	public void Execute(ClaimActionCollection parameter)
	{
		parameter.MapJsonSubKey(_claim, _key.Key, _key.Element, _type);
	}
}