using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using System.Security.Claims;

namespace DragonSpark.Presentation.Security.Identity;

sealed class CurrentPrincipal : ICurrentPrincipal
{
	readonly ICurrentContext _accessor;

	public CurrentPrincipal(ICurrentContext accessor) => _accessor = accessor;

	public ClaimsPrincipal Get() => _accessor.Get().User;
}