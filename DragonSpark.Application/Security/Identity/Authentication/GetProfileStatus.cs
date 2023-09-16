using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class GetProfileStatus : ISelect<CurrentProfileStateInput, ProfileStatus>
{
	public static GetProfileStatus Default { get; } = new();

	GetProfileStatus() : this(IdentityConstants.ApplicationScheme) {}

	readonly string _scheme;

	public GetProfileStatus(string scheme) => _scheme = scheme;

	public ProfileStatus Get(CurrentProfileStateInput parameter)
	{
		var (principal, user) = parameter;
		return principal.IsAuthenticated()
			       ? principal.Identity?.AuthenticationType == _scheme
				         ? user is not null
					           ? user.EmailConfirmed ? ProfileStatus.Confirmed : ProfileStatus.Confirming
					           : ProfileStatus.Invalid
				         : ProfileStatus.Authenticated
			       : ProfileStatus.Anonymous;
	}
}