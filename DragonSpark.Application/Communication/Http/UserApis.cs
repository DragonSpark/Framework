using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Communication.Http;

public sealed class UserApis<T> : ReferenceValueStore<ClaimsPrincipal, T> where T : class
{
	public UserApis(IBearer bearer, Func<T> client)
		: base(new AuthenticatedApi<T>(new ApplicationUserAwareBearer(bearer), client)) {}
}