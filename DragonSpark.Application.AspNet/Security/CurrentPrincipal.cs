using DragonSpark.Application.Security.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security;

sealed class CurrentPrincipal : ICurrentPrincipal
{
	readonly ICurrentContext _accessor;

	public CurrentPrincipal(ICurrentContext accessor) => _accessor = accessor;

	public ClaimsPrincipal Get() => _accessor.Get().User;
}