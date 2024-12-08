using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public static class Extensions
{
	public static ProfileStatus Get<T>(this ISelect<CurrentProfileStateInput, ProfileStatus> @this,
	                                   AuthenticationState<T> parameter) where T : IdentityUser
		=> @this.Get(new(parameter.User, parameter.Profile));
}