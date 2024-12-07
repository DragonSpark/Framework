using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System;
using System.Text.Json;

namespace DragonSpark.Application.Security.Identity.Claims.Actions;

public class CustomClaimAction : IClaimAction
{
	readonly string                    _claim;
	readonly string                    _element;
	readonly Func<JsonElement, string> _select;

	protected CustomClaimAction(string claim, string element, Func<JsonElement, string> select)
	{
		_claim   = claim;
		_element = element;
		_select  = select;
	}

	public void Execute(ClaimActionCollection parameter)
	{
		parameter.MapCustomJson(_claim, _element, _select);
	}
}