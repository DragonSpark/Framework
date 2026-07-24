using DragonSpark.Model.Selection.Stores;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.State;

sealed class ClaimPrincipalToken : ReferenceValueStore<ClaimsPrincipal, object>
{
	public static ClaimPrincipalToken Default { get; } = new();

	ClaimPrincipalToken() : base(x => new()) {}
}