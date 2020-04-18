using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose
{
	public sealed class OperationCallbackContext : IResult<EventCallback>
	{
		public static implicit operator EventCallback(OperationCallbackContext instance) => instance.Get();

		readonly IComponent _receiver;
		readonly IOperation _operation;

		public OperationCallbackContext(IComponent receiver, IOperation operation)
		{
			_receiver  = receiver;
			_operation = operation;
		}

		public OperationCallbackContext GuardEntry() => GuardEntry(TimeSpan.FromSeconds(1));

		public OperationCallbackContext GuardEntry(TimeSpan duration)
			=> new OperationCallbackContext(_receiver, new ThrottleEntryOperation(_operation, duration));

		public OperationCallbackContext UpdateActivity() => UpdateActivity(_receiver);

		public OperationCallbackContext UpdateActivity(IComponent subject)
			=> new OperationCallbackContext(_receiver, new ActivityAwareOperation(_operation, subject));

		public EventCallback Get() => EventCallback.Factory.Create(_receiver, _operation.Promote);
	}

	public sealed class OperationCallbackContext<T> : IResult<EventCallback<T>>
	{
		public static implicit operator EventCallback<T>(OperationCallbackContext<T> instance) => instance.Get();

		readonly IComponent    _receiver;
		readonly IOperation<T> _operation;

		public OperationCallbackContext(IComponent receiver, IOperation<T> operation)
		{
			_receiver  = receiver;
			_operation = operation;
		}

		public OperationCallbackContext<T> GuardEntry() => GuardEntry(TimeSpan.FromSeconds(1));

		public OperationCallbackContext<T> GuardEntry(TimeSpan duration)
			=> new OperationCallbackContext<T>(_receiver, new ThrottleEntryOperation<T>(_operation, duration));

		public OperationCallbackContext<T> UpdateActivity() => UpdateActivity(_receiver);

		public OperationCallbackContext<T> UpdateActivity(IComponent subject)
			=> new OperationCallbackContext<T>(_receiver, new ActivityAwareOperation<T>(_operation, subject));

		public EventCallback<T> Get()
			=> EventCallback.Factory.Create(_receiver, new Func<T, Task>(_operation.Promote));
	}
}