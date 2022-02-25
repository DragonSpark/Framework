using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CreateCircuitRecord : ISelecting<Circuit, CircuitRecord>
{
	readonly NavigationManager           _navigation;
	readonly AuthenticationStateProvider _authentication;
	readonly IConnectionIdentifier       _identifier;

	public CreateCircuitRecord(NavigationManager navigation, AuthenticationStateProvider authentication,
	                           IConnectionIdentifier identifier)
	{
		_navigation     = navigation;
		_authentication = authentication;
		_identifier     = identifier;
	}

	public async ValueTask<CircuitRecord> Get(Circuit parameter)
	{
		var state      = await _authentication.GetAuthenticationStateAsync().ConfigureAwait(false);
		var identifier = _identifier.Get().Verify();
		var result     = new CircuitRecord(identifier, parameter, _navigation, state.User);
		return result;
	}
}