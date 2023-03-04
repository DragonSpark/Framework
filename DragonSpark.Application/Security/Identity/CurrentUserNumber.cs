namespace DragonSpark.Application.Security.Identity;

sealed class CurrentUserNumber : ICurrentUserNumber
{
	readonly ICurrentUserIdentity _identity;

	public CurrentUserNumber(ICurrentUserIdentity identity) => _identity = identity;

	public uint Get() => uint.Parse(_identity.Get());
}