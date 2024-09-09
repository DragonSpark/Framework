using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose;

public sealed class OperationCallbackComposer : IResult<EventCallback>
{
	public static implicit operator EventCallback(OperationCallbackComposer instance) => instance.Get();

	readonly object             _receiver;
	readonly IOperation         _operation;

	public OperationCallbackComposer(object receiver, IOperation operation)
	{
		_receiver  = receiver;
		_operation = operation;
	}

	public OperationCallbackComposer Block() => Block(TimeSpan.FromSeconds(1));

	public OperationCallbackComposer Block(TimeSpan duration)
		=> new(_receiver, new BlockingEntryOperation(_operation, duration));

	public OperationCallbackComposer Monitoring(Switch subject)
		=> new(_receiver, new MonitoredOperation(_operation, subject));

	public OperationCallbackComposer UpdateActivity(IActivityReceiver receiver)
		=> UpdateActivity(receiver, ActivityOptions.Default);

	public OperationCallbackComposer UpdateActivity(IActivityReceiver receiver, ActivityOptions input)
		=> new(receiver, new ActivityAwareOperation(_operation, receiver, input));

	public OperationCallbackComposer Watching(IRenderState parameter)
		=> new(_receiver, new ActiveRenderAwareOperation(_operation, parameter));

	public EventCallback Get() => EventCallback.Factory.Create(_receiver, _operation.Allocate);
}

public sealed class OperationCallbackComposer<T> : IResult<EventCallback<T>>
{
	public static implicit operator EventCallback<T>(OperationCallbackComposer<T> instance) => instance.Get();

	readonly object        _receiver;
	readonly IOperation<T> _operation;

	public OperationCallbackComposer(object receiver, IOperation<T> operation)
	{
		_receiver  = receiver;
		_operation = operation;
	}

	public OperationCallbackComposer<T> Block() => Block(TimeSpan.FromSeconds(1.5));

	public OperationCallbackComposer<T> Block(TimeSpan duration)
		=> new(_receiver, new BlockingEntryOperation<T>(_operation, duration));

	public OperationCallbackComposer<T> UpdateActivity(IActivityReceiver receiver)
		=> UpdateActivity(receiver, ActivityOptions.Default);

	public OperationCallbackComposer<T> UpdateActivity(IActivityReceiver receiver, ActivityOptions options)
		=> new(receiver, new ActivityAwareOperation<T>(_operation, receiver, options));

	public EventCallback<T> Get()
		=> EventCallback.Factory.Create(_receiver, new Func<T, Task>(_operation.Allocate));

	public EventCallback Adapt()
		=> EventCallback.Factory.Create(_receiver, Start.A.Selection<object>()
		                                                .By.CastDown<T>()
		                                                .Select(_operation)
		                                                .Then()
		                                                .Allocate());
}