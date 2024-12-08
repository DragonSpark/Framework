﻿using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.AspNet.Security.Identity;
using System.Security.Claims;

namespace DragonSpark.Presentation.Security.Identity;

sealed class CurrentPrincipal : ICurrentPrincipal
{
	readonly AuthenticationStatePrincipal _state;
	readonly ICurrentContext              _accessor;

	public CurrentPrincipal(AuthenticationStatePrincipal state, ICurrentContext accessor)
	{
		_state    = state;
		_accessor = accessor;
	}

	public ClaimsPrincipal Get() => _state.Get() ?? _accessor.Get().User;
}