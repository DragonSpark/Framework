using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class InitializeAuthentication : IInitializeAuthentication
{
	readonly IAuthenticationStateMonitor _monitor;

	public InitializeAuthentication(IAuthenticationStateMonitor monitor) => _monitor = monitor;

	public ValueTask Get(ClaimsPrincipal parameter) => _monitor.Get();
}