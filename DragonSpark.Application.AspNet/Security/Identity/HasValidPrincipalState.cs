﻿using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class HasValidPrincipalState<T> : IHasValidPrincipalState where T : class
{
	readonly IAuthentications<T> _authentications;

	public HasValidPrincipalState(IAuthentications<T> authentications) => _authentications = authentications;

	public async ValueTask<bool> Get(ClaimsPrincipal parameter)
	{
		using var session = _authentications.Get();
		var       user    = await session.Subject.ValidateSecurityStampAsync(parameter).Await();
		var       result  = user != null;
		return result;
	}
}