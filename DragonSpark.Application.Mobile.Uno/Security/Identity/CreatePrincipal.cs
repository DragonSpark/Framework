using System.Security.Claims;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Identity;

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