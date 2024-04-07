namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class CurrentBearer : ICurrentBearer
{
	readonly ICurrentPrincipal _principal;
	readonly Bearer            _bearer;

	public CurrentBearer(ICurrentPrincipal principal, Bearer bearer)
	{
		_principal = principal;
		_bearer    = bearer;
	}

	public string Get() => _bearer.Get(_principal.Get());
}