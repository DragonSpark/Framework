namespace DragonSpark.Application.Security.Identity;

sealed class CurrentUserName : ICurrentUserName
{
	readonly ICurrentPrincipal _principal;

	public CurrentUserName(ICurrentPrincipal principal) => _principal = principal;

	public string Get() => _principal.Get().UserName();
}