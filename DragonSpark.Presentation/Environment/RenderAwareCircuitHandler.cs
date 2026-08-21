using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DragonSpark.Presentation.Environment;

sealed class RenderAwareCircuitHandler : CircuitHandler
{
	readonly RenderStateStore _store;

	public RenderAwareCircuitHandler(RenderStateStore store) => _store = store;

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		switch (_store.Get())
		{
			case RenderState.Default:
				_store.Execute(RenderState.Connected);
				break;
		}
		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}

	public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		switch (_store.Get())
		{
			case RenderState.Paused:
				_store.Execute(RenderState.Connected);
				break;
		}
		
		return base.OnConnectionUpAsync(circuit, cancellationToken);
	}

	public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Paused);
		return base.OnConnectionDownAsync(circuit, cancellationToken);
	}

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Destroyed);
		return base.OnCircuitClosedAsync(circuit, cancellationToken);
	}
}