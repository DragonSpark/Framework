using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public sealed class IsExternalPrincipal : InverseCondition<ClaimsPrincipal>
{
	public static IsExternalPrincipal Default { get; } = new();

	IsExternalPrincipal() : base(IsApplicationPrincipal.Default) {}
}