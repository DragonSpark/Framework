using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;

namespace DragonSpark.Presentation.Security.Identity;

sealed class AuthenticationStatePrincipal : IResult<ClaimsPrincipal?>
{
	readonly AuthenticationStateProvider _authentication;
	readonly string                      _message;

	public AuthenticationStatePrincipal(AuthenticationStateProvider authentication)
		: this(authentication, Properties.Resources.AuthenticationStatePrincipalMessage) {}

	public AuthenticationStatePrincipal(AuthenticationStateProvider authentication, string message)
	{
		_authentication = authentication;
		_message        = message;
	}

	public ClaimsPrincipal? Get()
	{
		try
		{
			var state = _authentication.GetAuthenticationStateAsync();
			if (state.IsCompletedSuccessfully)
			{
				return state.Result.User;
			}
		}
		catch (InvalidOperationException e) when (e.Message.StartsWith(_message)) {}

		return null;
	}
}