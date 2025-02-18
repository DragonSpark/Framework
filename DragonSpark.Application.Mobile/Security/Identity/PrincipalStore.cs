using DragonSpark.Model.Selection.Stores;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class PrincipalStore : ReferenceValueStore<string, ClaimsPrincipal>
{
	public PrincipalStore(string access) : this(access, CreatePrincipal.Default.Get) {}

	public PrincipalStore(string access, Func<IdentityPayload, ClaimsPrincipal> load)
		: base(x => load(new(access, x))) {}
}