using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DragonSpark.Presentation.Compose;

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

	public OperationCallbackContext Handle<T>(IExceptions exceptions) => Handle(exceptions, A.Type<T>());

	public OperationCallbackContext Handle(IExceptions exceptions, Type? reportedType = null)
	{
		var receiver  = _receiver.Verify();
		var operation = new ExceptionAwareOperation(reportedType ?? receiver.GetType(), exceptions, _method);
		var result    = new OperationCallbackContext(receiver, operation);
		return result;
	}

	public OperationCallbackContext Throttle() => Throttle(TimeSpan.FromSeconds(1));

	public OperationCallbackContext Throttle(TimeSpan window)
	{
		var throttling = new Throttling(Start.A.Result(_method).Then().Structure(), window);
		var operation  = throttling.Then().Operation().Out();
		var result     = new OperationCallbackContext(_receiver.Verify(), operation);
		return result;
	}

	public OperationCallbackContext Throttle(ICondition<None> when, TimeSpan @for)
		=> Throttle(when.Then().Operation().Out(), @for);

	public OperationCallbackContext Throttle(IDepending<None> when, TimeSpan @for)
	{
		var operate   = Start.A.Result(_method).Then().Structure();
		var operation = new Throttling(operate, @for).Then().Operation();
		var result = new OperationCallbackContext(_receiver.Verify(), new Validating(when.Await, operation, operate));
		return result;
	}

	public OperationCallbackContext Block() => BlockFor(TimeSpan.FromSeconds(1));

	public OperationCallbackContext BlockFor(TimeSpan duration)
		=> new(_receiver.Verify(),
		       new BlockingEntryOperation(Start.A.Result(_method).Then().Structure().Out(), duration));

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

	public OperationCallbackContext<T> Throttle() => Throttle(TimeSpan.FromSeconds(1));

	public OperationCallbackContext<T> Throttle(TimeSpan window)
	{
#pragma warning disable CS8714
		var throttling = new Throttling<T>(new Table<T, Timer>(), window);
#pragma warning restore CS8714
		var operation = new ThrottleOperation<T>(throttling, Start.A.Selection(_method).Then().Structure());
		var result    = new OperationCallbackContext<T>(_receiver.Verify(), operation);
		return result;
	}

	public OperationCallbackContext<T> UpdateActivity()
	{
		var receiver  = _receiver.Verify();
		var body      = Start.A.Selection(_method).Then().Structure().Out();
		var operation = new ActivityAwareOperation<T>(body, receiver);
		var result    = new OperationCallbackContext<T>(receiver, operation);
		return result;
	}

	public OperationCallbackContext<T> Handle<TReported>(IExceptions exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationCallbackContext<T> Handle(IExceptions exceptions, Type? reportedType = null)
	{
		var receiver  = _receiver.Verify();
		var operation = new ExceptionAwareOperation<T>(reportedType ?? receiver.GetType(), exceptions, _method);
		var result    = new OperationCallbackContext<T>(receiver, operation);
		return result;
	}

	public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver!, _method);
}