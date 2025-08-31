using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareCircuitHandler : CircuitHandler
{
	readonly IEstablishContext          _establish;
	readonly IMutable<HubCallerContext?> _source;

	public ContextAwareCircuitHandler(IEstablishContext establish) : this(establish, AmbientContext.Default) {}

	public ContextAwareCircuitHandler(IEstablishContext establish, IMutable<HubCallerContext?> source)
	{
		_establish = establish;
		_source     = source;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		var context = _source.Get().Verify().GetHttpContext().Verify();
		_establish.Execute(context);
		return Task.CompletedTask;
	}
}