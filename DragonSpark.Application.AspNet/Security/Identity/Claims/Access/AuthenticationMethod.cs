﻿using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public sealed class AuthenticationMethod : ReadClaim
{
	public static AuthenticationMethod Default { get; } = new();

	AuthenticationMethod() : base(ClaimTypes.AuthenticationMethod) {}
}