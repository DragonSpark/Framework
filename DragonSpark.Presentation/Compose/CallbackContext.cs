using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose
{
	public sealed class CallbackContext : IResult<EventCallback>
	{
		public static implicit operator EventCallback(CallbackContext instance) => instance.Get();

		readonly IComponent _receiver;
		readonly Func<Task> _method;

		public CallbackContext(Func<Task> method) : this(method.Target as IComponent, method) {}

		public CallbackContext(IComponent receiver, Func<Task> method)
		{
			_receiver = receiver;
			_method   = method;
		}

		public CallbackContext Using(IComponent receiver) => new CallbackContext(receiver, _method);

		public OperationCallbackContext Handle(IExceptions exceptions)
			=> new OperationCallbackContext(_receiver,
			                                new ExceptionAwareOperation(_receiver.GetType(), exceptions, _method));

		public EventCallback Get() => EventCallback.Factory.Create(_receiver, _method);
	}

	public sealed class CallbackContext<T> : IResult<EventCallback<T>>
	{
		public static implicit operator EventCallback<T>(CallbackContext<T> instance) => instance.Get();

		readonly IComponent    _receiver;
		readonly Func<T, Task> _method;

		public CallbackContext(Func<T, Task> method) : this(method.Target as IComponent, method) {}

		public CallbackContext(IComponent receiver, Func<T, Task> method)
		{
			_receiver = receiver;
			_method   = method;
		}

		public CallbackContext<T> Using(IComponent receiver) => new CallbackContext<T>(receiver, _method);

		public OperationCallbackContext<T> Handle(IExceptions exceptions)
			=> new OperationCallbackContext<T>(_receiver,
			                                   new ExceptionAwareOperation<T>(_receiver.GetType(), exceptions,
			                                                                  _method));

		public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver, _method);
	}
}