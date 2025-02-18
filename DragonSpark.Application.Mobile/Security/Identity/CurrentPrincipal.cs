using DragonSpark.Application.Security.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class CurrentPrincipal : ICurrentPrincipal
{
	readonly IPrincipalAccess _access;
	readonly ClaimsPrincipal  _default;

	public CurrentPrincipal(IPrincipalAccess access) : this(access, AnonymousPrincipal.Default) {}

	public CurrentPrincipal(IPrincipalAccess access, ClaimsPrincipal @default)
	{
		_access  = access;
		_default = @default;
	}

	public ClaimsPrincipal Get() => _access.Get() ?? _default;
}