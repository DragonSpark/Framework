using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Presentation.Connections.Initialization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Security.Identity;

sealed class SignOut : ISignOut
{
	readonly ISignOut               _previous;
	readonly ClearCurrentIdentifier _clear;

	public SignOut(ISignOut previous, ClearCurrentIdentifier clear)
	{
		_previous = previous;
		_clear    = clear;
	}

	public ValueTask Get(ClaimsPrincipal parameter)
	{
		_clear.Execute();
		return _previous.Get(parameter);
	}
}