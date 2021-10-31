using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Navigation.Security.Identity;

public sealed class DefaultCurrentExternalLogin : CurrentExternalLogin
{
	public DefaultCurrentExternalLogin(DefaultExternalLogin @select, CurrentAuthenticationMethod current)
		: base(@select, current) {}
}