using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Actions;

public class ClaimAction : IClaimAction
{
	readonly string _element, _claim, _type;

	public ClaimAction(string claim, string element, string type = "string")
	{
		_element = element;
		_claim   = claim;
		_type    = type;
	}

	public void Execute(ClaimActionCollection parameter)
	{
		parameter.MapJsonKey(_claim, _element, _type);
	}
}