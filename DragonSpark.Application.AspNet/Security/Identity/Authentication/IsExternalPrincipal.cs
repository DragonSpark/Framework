using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class IsExternalPrincipal : InverseCondition<ClaimsPrincipal>
{
	public static IsExternalPrincipal Default { get; } = new();

	IsExternalPrincipal() : base(IsApplicationPrincipal.Default) {}
}