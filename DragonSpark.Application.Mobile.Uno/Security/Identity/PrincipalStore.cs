using System;
using System.Security.Claims;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

sealed class PrincipalStore : ReferenceValueStore<string, ClaimsPrincipal>
{
	public PrincipalStore(string access) : this(access, CreatePrincipal.Default.Get) {}

	public PrincipalStore(string access, Func<IdentityPayload, ClaimsPrincipal> load)
		: base(x => load(new(access, x))) {}
}