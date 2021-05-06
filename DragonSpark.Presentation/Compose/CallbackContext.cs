using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose
{
	public sealed class CallbackContext : IResult<EventCallback>
	{
		public static implicit operator EventCallback(CallbackContext instance) => instance.Get();

		readonly object?    _receiver;
		readonly Func<Task> _method;

		public CallbackContext(Func<Task> method) : this(method.Target, method) {}

		public CallbackContext(object? receiver, Func<Task> method)
		{
			_receiver = receiver;
			_method   = method;
		}

		public CallbackContext Using(object receiver) => new CallbackContext(receiver, _method);

		public OperationCallbackContext Handle(IExceptions exceptions)
		{
			var receiver  = _receiver.Verify();
			var operation = new ExceptionAwareOperation(receiver.GetType(), exceptions, _method);
			var result    = new OperationCallbackContext(receiver, operation);
			return result;
		}

		public OperationCallbackContext UpdateActivity()
		{
			var receiver  = _receiver.Verify();
			var body      = Start.A.Result(_method).Then().Structure().Out();
			var operation = new ActivityAwareOperation(body, receiver);
			var result    = new OperationCallbackContext(receiver, operation);
			return result;
		}

		public CallbackContext Append(Action next) => Append(Start.A.Command(next).Operation().Allocate());

		public CallbackContext Append(Func<Task> next)
			=> new CallbackContext(_receiver ?? next.Target, _method.Start().Then().Append(next));

		public CallbackContext Append(Operate next)
			=> new CallbackContext(_receiver ?? next.Target, _method.Start().Then().Structure().Append(next).Allocate());

		public EventCallback Get() => EventCallback.Factory.Create(_receiver!, _method);
	}

	public class CallbackContext<T> : IResult<EventCallback<T>>
	{
		public static implicit operator EventCallback<T>(CallbackContext<T> instance) => instance.Get();

		readonly object?       _receiver;
		readonly Func<T, Task> _method;

		public CallbackContext(Func<T, Task> method) : this(method.Target, method) {}

		public CallbackContext(object? receiver, Func<T, Task> method)
		{
			_receiver = receiver;
			_method   = method;
		}

		public CallbackContext<T> Using(object receiver) => new CallbackContext<T>(receiver, _method);

		public OperationCallbackContext<T> UpdateActivity()
		{
			var receiver  = _receiver.Verify();
			var body      = Start.A.Selection(_method).Then().Structure().Out();
			var operation = new ActivityAwareOperation<T>(body, receiver);
			var result    = new OperationCallbackContext<T>(receiver, operation);
			return result;
		}

		public OperationCallbackContext<T> Handle(IExceptions exceptions)
		{
			var receiver  = _receiver.Verify();
			var operation = new ExceptionAwareOperation<T>(receiver.GetType(), exceptions, _method);
			var result    = new OperationCallbackContext<T>(receiver, operation);
			return result;
		}

		public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver!, _method);
	}
}