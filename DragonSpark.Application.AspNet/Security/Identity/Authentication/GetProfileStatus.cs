using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public sealed class GetProfileStatus : ISelect<CurrentProfileStateInput, ProfileStatus>
{
	public static GetProfileStatus Default { get; } = new();

	GetProfileStatus() : this(IsApplicationPrincipal.Default) {}

	readonly ICondition<ClaimsPrincipal> _application;

	public GetProfileStatus(ICondition<ClaimsPrincipal> application) => _application = application;

	public ProfileStatus Get(CurrentProfileStateInput parameter)
	{
		var (principal, user) = parameter;
		return principal.IsAuthenticated()
			       ? _application.Get(principal)
				         ? user is not null
					           ? user.Email is null  ? ProfileStatus.InputRequired :
					             user.EmailConfirmed ? ProfileStatus.Confirmed : ProfileStatus.Confirming
					           : ProfileStatus.Invalid
				         : ProfileStatus.Authenticated
			       : ProfileStatus.Anonymous;
	}
}