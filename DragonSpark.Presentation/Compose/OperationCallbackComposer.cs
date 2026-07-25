using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Compose;

public sealed class OperationCallbackComposer : IResult<EventCallback>
{
	public static implicit operator EventCallback(OperationCallbackComposer instance) => instance.Get();

	readonly object     _receiver;
	readonly IOperation _operation;

	public OperationCallbackComposer(object receiver, IOperation operation)
	{
		_receiver  = receiver;
		_operation = operation;
	}

	public OperationCallbackComposer Block() => Block(false);

	public OperationCallbackComposer Block(Switch monitor)
		=> new(_receiver, new BlockOperation(_operation, monitor));

	public OperationCallbackComposer DurationBlock() => DurationBlock(TimeSpan.FromSeconds(1));

	public OperationCallbackComposer DurationBlock(TimeSpan duration)
		=> new(_receiver, new DurationBlockOperation(_operation, duration));

	public OperationCallbackComposer Monitoring(Switch subject)
		=> new(_receiver, new MonitoredOperation(_operation, subject));

	public OperationCallbackComposer UpdateActivity(IActivityReceiver receiver)
		=> UpdateActivity(receiver, ActivityOptions.Default);

	public OperationCallbackComposer UpdateActivity(IActivityReceiver receiver, CancelAwareActivityOptions options)
		=> new(receiver.Target(_receiver), 
		       new CancelAwareOperation(new ActivityAwareOperation(_operation, receiver, options), options));

	public OperationCallbackComposer UpdateActivity(IActivityReceiver receiver, ActivityOptions options)
		=> new(receiver.Target(_receiver), new ActivityAwareOperation(_operation, receiver, options));

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

	public OperationCallbackComposer<T> Block() => Block(false);

	public OperationCallbackComposer<T> Block(Switch monitor)
		=> new(_receiver, new BlockOperation<T>(_operation, monitor));

	public OperationCallbackComposer<T> DurationBlock() => DurationBlock(TimeSpan.FromSeconds(1.5));

	public OperationCallbackComposer<T> DurationBlock(TimeSpan duration)
		=> new(_receiver, new DurationBlockOperation<T>(_operation, duration));

	public OperationCallbackComposer<T> UpdateActivity(IActivityReceiver receiver)
		=> UpdateActivity(receiver, ActivityOptions.Default);

	public OperationCallbackComposer<T> UpdateActivity(IActivityReceiver receiver, ActivityOptions options)
		=> new(receiver.Target(_receiver), new ActivityAwareOperation<T>(_operation, receiver, options));

	public OperationCallbackComposer<T> Monitoring(Switch subject)
		=> new(_receiver, new MonitoredOperation<T>(_operation, subject));

	public EventCallback<T> Get()
		=> EventCallback.Factory.Create(_receiver, new Func<T, Task>(_operation.Allocate));

	public EventCallback Adapt()
		=> EventCallback.Factory.Create(_receiver, Start.A.Selection<object>()
		                                                .By.CastDown<T>()
		                                                .Select(_operation)
		                                                .Then()
		                                                .Allocate());
}