using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CreateCircuitRecord : ISelecting<Circuit, CircuitRecord>
{
	readonly NavigationManager           _navigation;
	readonly AuthenticationStateProvider _authentication;

	public CreateCircuitRecord(NavigationManager navigation, AuthenticationStateProvider authentication)
	{
		_navigation     = navigation;
		_authentication = authentication;
	}

	public async ValueTask<CircuitRecord> Get(Circuit parameter)
	{
		var state = await _authentication.GetAuthenticationStateAsync().Off();
		return new(parameter, _navigation, state.User);
	}
}