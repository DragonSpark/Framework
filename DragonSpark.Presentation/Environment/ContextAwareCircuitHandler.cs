using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareCircuitHandler : CircuitHandler
{
	readonly IEstablishContext           _establish;
	readonly IAlteration<HttpContext>    _clone;
	readonly IMutable<HubCallerContext?> _source;

	public ContextAwareCircuitHandler(IEstablishContext establish, ICopyContext copy)
		: this(establish, copy, AmbientContext.Default) {}

	public ContextAwareCircuitHandler(IEstablishContext establish, IAlteration<HttpContext> clone,
	                                  IMutable<HubCallerContext?> source)
	{
		_establish = establish;
		_clone     = clone;
		_source    = source;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		var original = _source.Get().Verify().GetHttpContext().Verify();
		var clone    = _clone.Get(original);
		_establish.Execute(clone);
		return Task.CompletedTask;
	}
}