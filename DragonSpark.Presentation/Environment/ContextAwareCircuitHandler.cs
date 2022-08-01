using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareCircuitHandler : CircuitHandler
{
	readonly IInitializeContext          _store;
	readonly IMutable<HubCallerContext?> _context;

	public ContextAwareCircuitHandler(IInitializeContext store) : this(store, AmbientContext.Default) {}

	public ContextAwareCircuitHandler(IInitializeContext store, IMutable<HubCallerContext?> context)
	{
		_store   = store;
		_context = context;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		var context = _context.Get().Verify().GetHttpContext().Verify();
		_store.Execute(context);
		return Task.CompletedTask;
	}
}