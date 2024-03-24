using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment;

sealed class ContextAwareCircuitHandler : CircuitHandler
{
	readonly IEstablishContext          _establish;
	readonly IMutable<HubCallerContext?> _source;
	readonly IHttpContextAccessor        _accessor;

	public ContextAwareCircuitHandler(IEstablishContext establish, IHttpContextAccessor accessor)
		: this(establish, AmbientContext.Default, accessor) {}

	public ContextAwareCircuitHandler(IEstablishContext establish, IMutable<HubCallerContext?> source,
	                                  IHttpContextAccessor accessor)
	{
		_establish = establish;
		_source     = source;
		_accessor   = accessor;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_accessor.HttpContext ??= _source.Get().Verify().GetHttpContext().Verify();
		_establish.Execute(_accessor.HttpContext);
		return Task.CompletedTask;
	}
}