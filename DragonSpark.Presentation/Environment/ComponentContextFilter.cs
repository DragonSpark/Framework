using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment;

sealed class ComponentContextFilter : IHubFilter
{
	readonly IMutable<HubCallerContext?> _store;

	[ActivatorUtilitiesConstructor]
	public ComponentContextFilter() : this(AmbientContext.Default) {}

	public ComponentContextFilter(IMutable<HubCallerContext?> store) => _store = store;

	public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext,
	                                                  Func<HubInvocationContext, ValueTask<object?>> next)
	{
		if (invocationContext.HubMethodName == "UpdateRootComponents" 
				&& invocationContext.Hub.GetType().Name == "ComponentHub")
		{
			using (_store.Assigned(invocationContext.Context))
			{
				return await next(invocationContext).Off();
			}
		}
		return await next(invocationContext).Off();
	}
}