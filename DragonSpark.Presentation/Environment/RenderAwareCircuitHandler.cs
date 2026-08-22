using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Eventing;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DragonSpark.Presentation.Environment;

sealed class RenderAwareCircuitHandler : CircuitHandler
{
	readonly RenderStateStore                  _store;
	readonly IPublisher<CircuitPausedMessage>  _paused;
	readonly IPublisher<CircuitResumedMessage> _resumed;

	public RenderAwareCircuitHandler(RenderStateStore store, IPublisher<CircuitPausedMessage> paused,
	                                 IPublisher<CircuitResumedMessage> resumed)
	{
		_store   = store;
		_paused  = paused;
		_resumed = resumed;
	}

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

	public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		switch (_store.Get())
		{
			case RenderState.Paused:
				_store.Execute(RenderState.Connected);
				await _resumed.Off(new(circuit.Id));
				break;
		}

		await base.OnConnectionUpAsync(circuit, cancellationToken).Off();
	}

	public override async Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Paused);
		await _paused.Off(new(circuit.Id));
		await base.OnConnectionDownAsync(circuit, cancellationToken).Off();
	}

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_store.Execute(RenderState.Destroyed);
		return base.OnCircuitClosedAsync(circuit, cancellationToken);
	}
}