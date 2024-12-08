﻿using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

public readonly record struct ProviderIdentity(string Provider, string Identity)
{
	public static implicit operator ProviderIdentity(ExternalLoginInfo instance) => instance.AsIdentity();
}