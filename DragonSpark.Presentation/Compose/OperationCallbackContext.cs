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

	readonly object     _receiver;
	readonly IOperation _operation;

	public OperationCallbackContext(object receiver, IOperation operation)
	{
		_receiver  = receiver;
		_operation = operation;
	}

	public OperationCallbackContext Using(object receiver) => new(receiver, _operation);

	public OperationCallbackContext Block() => Block(TimeSpan.FromSeconds(1));

	public OperationCallbackContext Block(TimeSpan duration)
		=> new(_receiver, new BlockingEntryOperation(_operation, duration));

	public OperationCallbackContext UpdateActivity()
		=> new(_receiver, new ActivityAwareOperation(_operation, _receiver));

	public OperationCallbackContext Watching(IRenderState parameter)
		=> new (_receiver, new ActiveRenderAwareOperation(_operation, parameter));

	public EventCallback Get() => EventCallback.Factory.Create(_receiver, _operation.Allocate);
}

public sealed class OperationCallbackContext<T> : IResult<EventCallback<T>>
{
	public static implicit operator EventCallback<T>(OperationCallbackContext<T> instance) => instance.Get();

	readonly object        _receiver;
	readonly IOperation<T> _operation;

	public OperationCallbackContext(object receiver, IOperation<T> operation)
	{
		_receiver  = receiver;
		_operation = operation;
	}

	public OperationCallbackContext<T> Using(object receiver) => new(receiver, _operation);

	public OperationCallbackContext<T> Block() => Block(TimeSpan.FromSeconds(1.5));

	public OperationCallbackContext<T> Block(TimeSpan duration)
		=> new(_receiver, new BlockingEntryOperation<T>(_operation, duration));

	public OperationCallbackContext<T> UpdateActivity()
		=> new(_receiver, new ActivityAwareOperation<T>(_operation, _receiver));

	public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver, new Func<T, Task>(_operation.Allocate));

	public EventCallback Adapt() => EventCallback.Factory.Create(_receiver,
	                                                             Start.A.Selection<object>()
	                                                                  .By.CastDown<T>()
	                                                                  .Select(_operation)
	                                                                  .Then()
	                                                                  .Allocate());
}