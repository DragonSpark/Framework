using DragonSpark.Application.Security.Identity.Bearer;
using System;

namespace DragonSpark.Application.Communication.Http;

public sealed class AuthenticatedApi<T> : User<T>
{
	public AuthenticatedApi(ApplicationUserAwareBearer bearer, Func<T> client) : base(bearer, client) {}
}