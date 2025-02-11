﻿using DragonSpark.Application.Security.Identity.Claims;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity;

public sealed class Identities : Select<ClaimsPrincipal, ProviderIdentity>
{
	public static Identities Default { get; } = new();

	Identities() : this(ExternalIdentityValue.Default, IdentityParser.Default) {}

	public Identities(IRequiredClaim identity, ISelect<string, ProviderIdentity> parser)
		: base(identity.Then().Select(parser)) {}
}