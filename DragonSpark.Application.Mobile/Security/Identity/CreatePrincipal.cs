using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class CreatePrincipal : ISelect<IdentityPayload, ClaimsPrincipal>
{
	public static CreatePrincipal Default { get; } = new();

	CreatePrincipal() : this(CreateIdentity.Default) {}

	readonly ISelect<string, ClaimsIdentity> _identity;

	public CreatePrincipal(ISelect<string, ClaimsIdentity> identity) => _identity = identity;

	public ClaimsPrincipal Get(IdentityPayload parameter)
	{
		var (access, identity) = parameter;
		return new([_identity.Get(access), _identity.Get(identity)]);
	}
}