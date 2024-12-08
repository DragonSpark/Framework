using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Model.Commands;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

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
		{
			var number = parameter.Number();
			if (number is not null)
			{
				_clear.Execute(number.Value);
			}
		}
		var current = _current.Get();
		if (current != parameter)
		{
			var number = current.Number();
			if (number is not null)
			{
				_clear.Execute(number.Value);
			}
		}
	}
}