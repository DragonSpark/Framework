using DragonSpark.Application.AspNet.Security.Identity.Claims.Access;
using DragonSpark.Text;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class IdentityProvider : IFormatter<ClaimsPrincipal>
{
	public static IdentityProvider Default { get; } = new();

	IdentityProvider() : this(ReadIdentityProvider.Default) {}

	readonly IReadClaim _read;

	public IdentityProvider(IReadClaim read) => _read = read;

	public string Get(ClaimsPrincipal parameter)
		=> _read.Read(parameter) ?? parameter.Identity?.AuthenticationType ?? throw new InvalidOperationException();
}