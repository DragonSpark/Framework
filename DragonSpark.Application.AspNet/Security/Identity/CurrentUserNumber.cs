namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class CurrentUserNumber : ICurrentUserNumber
{
	readonly ICurrentPrincipal _current;

	public CurrentUserNumber(ICurrentPrincipal current) => _current = current;

	public uint Get() => _current.Get().Number() ?? 0;
}