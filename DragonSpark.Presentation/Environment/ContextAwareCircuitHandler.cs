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
	readonly IInitializeContext          _initialize;
	readonly IMutable<HubCallerContext?> _source;
	readonly IHttpContextAccessor        _accessor;

	public ContextAwareCircuitHandler(IInitializeContext initialize, IHttpContextAccessor accessor)
		: this(initialize, AmbientContext.Default, accessor) {}

	public ContextAwareCircuitHandler(IInitializeContext initialize, IMutable<HubCallerContext?> source,
	                                  IHttpContextAccessor accessor)
	{
		_initialize = initialize;
		_source     = source;
		_accessor   = accessor;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		var context = _source.Get().Verify().GetHttpContext().Verify();
		_accessor.HttpContext ??= context;
		_initialize.Execute(context);
		return Task.CompletedTask;
	}
}