using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose
{
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

		public OperationCallbackContext Using(object receiver) => new OperationCallbackContext(receiver, _operation);

		public OperationCallbackContext Throttle() => Throttle(TimeSpan.FromSeconds(1));

		public OperationCallbackContext Throttle(TimeSpan duration)
			=> new OperationCallbackContext(_receiver, new ThrottleEntryOperation(_operation, duration));

		public OperationCallbackContext UpdateActivity()
			=> new OperationCallbackContext(_receiver, new ActivityAwareOperation(_operation, _receiver));

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

		public OperationCallbackContext<T> Using(object receiver)
			=> new OperationCallbackContext<T>(receiver, _operation);

		public OperationCallbackContext<T> Throttle() => Throttle(TimeSpan.FromSeconds(1));

		public OperationCallbackContext<T> Throttle(TimeSpan duration)
			=> new OperationCallbackContext<T>(_receiver, new ThrottleEntryOperation<T>(_operation, duration));

		public OperationCallbackContext<T> UpdateActivity()
			=> new OperationCallbackContext<T>(_receiver, new ActivityAwareOperation<T>(_operation, _receiver));

		public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver, new Func<T, Task>(_operation.Allocate));

		public EventCallback Adapt() => EventCallback.Factory.Create(_receiver,
		                                                             Start.A.Selection<object>()
		                                                                  .By.CastDown<T>()
		                                                                  .Select(_operation)
		                                                                  .Then()
		                                                                  .Allocate());
	}
}