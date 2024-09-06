using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Presentation.Components.Diagnostics;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

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

	public OperationCallbackContext Handle<T>(IExceptions exceptions) => Handle(exceptions, A.Type<T>());

	public OperationCallbackContext Handle(IExceptions exceptions, Type? reportedType = null)
	{
		var receiver  = _receiver.Verify();
		var operation = new ExceptionAwareOperation(reportedType ?? receiver.GetType(), exceptions, _method);
		var result    = new OperationCallbackContext(receiver, operation);
		return result;
	}

	public OperationCallbackContext UpdateActivity(IActivityReceiver receiver)
	{
		var body      = _method.Start().Then().Structure().Out();
		var operation = new ActivityAwareOperation(body, receiver);
		return new(receiver, operation);
	}

	public OperationCallbackContext BlockFor(TimeSpan duration)
		=> new(_receiver.Verify(),
		       new BlockingEntryOperation(new Allocated(_method).Then().Structure().Out(), duration));

	public CallbackContext Append(Action next) => Append(Start.A.Command(next).Operation().Allocate());

	public CallbackContext Append(Func<Task> next)
		=> new(_receiver ?? next.Target, _method.Start().Then().Append(next));

	public CallbackContext Append(Operate next)
		=> new(_receiver ?? next.Target, _method.Start().Then().Structure().Append(next).Allocate());

	public CallbackContext Watching(IRenderState parameter)
		=> new(new ActiveRenderAwareOperation(_method.Start().Then().Structure().Out(), parameter).Allocate);

	public EventCallback Get() => EventCallback.Factory.Create(_receiver!, _method);
}

public class CallbackContext<T> : IResult<EventCallback<T>> // TODO: Rename
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

	public OperationCallbackContext<T> UpdateActivity(IActivityReceiver receiver)
	{
		var body      = Start.A.Selection(_method).Then().Structure().Out();
		var operation = new ActivityAwareOperation<T>(body, receiver);
		return new(receiver, operation);
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

	public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver.Verify(), _method);

	public CallbackContext<T> Hide() => new(EmptyReceiver.Default, Start.A.Selection(_method).Then());
}