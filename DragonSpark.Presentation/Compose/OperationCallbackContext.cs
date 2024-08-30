using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose;

public sealed class OperationCallbackContext : IResult<EventCallback>
{
	public static implicit operator EventCallback(OperationCallbackContext instance) => instance.Get();

	readonly object             _receiver;
	readonly IOperation         _operation;
	readonly IActivityReceiver? _activity;

	public OperationCallbackContext(object receiver, IOperation operation, IActivityReceiver? activity = null)
	{
		_receiver  = receiver;
		_operation = operation;
		_activity  = activity;
	}

	// TODO: Move to UpdateActivity
	public OperationCallbackContext Using(IActivityReceiver receiver) => new(_receiver, _operation, receiver);

	public OperationCallbackContext Block() => Block(TimeSpan.FromSeconds(1));

	public OperationCallbackContext Block(TimeSpan duration)
		=> new(_receiver, new BlockingEntryOperation(_operation, duration), _activity);

	public OperationCallbackContext Monitoring(Switch subject)
		=> new(_receiver, new MonitoredOperation(_operation, subject), _activity);

	public OperationCallbackContext UpdateActivity() => UpdateActivity(ActivityReceiverInput.Default);

	public OperationCallbackContext UpdateActivity(ActivityReceiverInput input)
		=> new(_receiver, new ActivityAwareOperation(_operation, _activity ?? _receiver, input), _activity);

	public OperationCallbackContext Watching(IRenderState parameter)
		=> new(_receiver, new ActiveRenderAwareOperation(_operation, parameter), _activity);

	public EventCallback Get()
		=> EventCallback.Factory.Create(_activity ?? _receiver, _operation.Allocate);
}

public sealed class OperationCallbackContext<T> : IResult<EventCallback<T>>
{
	public static implicit operator EventCallback<T>(OperationCallbackContext<T> instance) => instance.Get();

	readonly object             _receiver;
	readonly IOperation<T>      _operation;

	public OperationCallbackContext(object receiver, IOperation<T> operation)
	{
		_receiver  = receiver;
		_operation = operation;
	}

	public OperationCallbackContext<T> Block() => Block(TimeSpan.FromSeconds(1.5));

	public OperationCallbackContext<T> Block(TimeSpan duration)
		=> new(_receiver, new BlockingEntryOperation<T>(_operation, duration));

	// TODO:
	public OperationCallbackContext<T> UpdateActivityWithRefresh() => UpdateActivity(ActivityReceiverInput.WithRefresh);

	// TODO: Move to UpdateActivity
	public OperationCallbackContext<T> Using(IActivityReceiver receiver) => new(receiver, _operation);

	public OperationCallbackContext<T> UpdateActivity() => UpdateActivity(ActivityReceiverInput.Default);

	public OperationCallbackContext<T> UpdateActivity(ActivityReceiverInput input)
		=> new(_receiver, new ActivityAwareOperation<T>(_operation, _receiver, input));

	public EventCallback<T> Get()
		=> EventCallback.Factory.Create(_receiver, new Func<T, Task>(_operation.Allocate));

	public EventCallback Adapt()
		=> EventCallback.Factory.Create(_receiver, Start.A.Selection<object>()
		                                                .By.CastDown<T>()
		                                                .Select(_operation)
		                                                .Then()
		                                                .Allocate());
}