﻿using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class StateAwareAddExternalSignin : IAddExternalSignin
{
	readonly IAddExternalSignin        _previous;
	readonly IClearAuthenticationState _clear;

	public StateAwareAddExternalSignin(IAddExternalSignin previous, IClearAuthenticationState clear)
	{
		_previous = previous;
		_clear    = clear;
	}

	public async ValueTask<IdentityResult?> Get(ClaimsPrincipal parameter)
	{
		var result = await _previous.Await(parameter);
		if (result?.Succeeded ?? false)
		{
			var number = parameter.Number();
			if (number is not null)
			{
				_clear.Execute(number.Value);
			}
		}

		return result;
	}
}