using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class ClearCurrentAuthentication : ICommand<ClaimsPrincipal>
{
	readonly ICurrentPrincipal         _current;
	readonly IClearAuthenticationState _clear;

	public ClearCurrentAuthentication(ICurrentPrincipal current, IClearAuthenticationState clear)
	{
		_current = current;
		_clear   = clear;
	}

	public void Execute(ClaimsPrincipal parameter)
	{
		_clear.Execute(parameter);
		var current = _current.Get();
		if (current != parameter)
		{
			_clear.Execute(current);
		}
	}
}