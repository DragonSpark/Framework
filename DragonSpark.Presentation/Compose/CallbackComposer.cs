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

public sealed class CallbackComposer : IResult<EventCallback>
{
	public static implicit operator EventCallback(CallbackComposer instance) => instance.Get();

	readonly object?    _receiver;
	readonly Func<Task> _method;

	public CallbackComposer(Func<Task> method) : this(method.Target, method) {}

	public CallbackComposer(object? receiver, Func<Task> method)
	{
		_receiver = receiver;
		_method   = method;
	}

	public OperationCallbackComposer Handle<T>(IExceptions exceptions) => Handle(exceptions, A.Type<T>());

	public OperationCallbackComposer Handle(IExceptions exceptions, Type? reportedType = null)
	{
		var receiver  = _receiver.Verify();
		var operation = new ExceptionAwareOperation(reportedType ?? receiver.GetType(), exceptions, _method);
		var result    = new OperationCallbackComposer(receiver, operation);
		return result;
	}

	public OperationCallbackComposer UpdateActivity(IActivityReceiver receiver)
	{
		var body      = _method.Start().Then().Structure().Out();
		var operation = new ActivityAwareOperation(body, receiver);
		return new(receiver, operation);
	}

	public OperationCallbackComposer BlockFor(TimeSpan duration)
		=> new(_receiver.Verify(),
		       new BlockingEntryOperation(new Allocated(_method).Then().Structure().Out(), duration));

	public CallbackComposer Append(Action next) => Append(Start.A.Command(next).Operation().Allocate());

	public CallbackComposer Append(Func<Task> next)
		=> new(_receiver ?? next.Target, _method.Start().Then().Append(next));

	public CallbackComposer Append(Operate next)
		=> new(_receiver ?? next.Target, _method.Start().Then().Structure().Append(next).Allocate());

	public CallbackComposer Watching(IRenderState parameter)
		=> new(new ActiveRenderAwareOperation(_method.Start().Then().Structure().Out(), parameter).Allocate);

	public CallbackComposer Hide() => new(EmptyReceiver.Default, _method.Start());

	public EventCallback Get() => EventCallback.Factory.Create(_receiver!, _method);
}

public class CallbackComposer<T> : IResult<EventCallback<T>>
{
	public static implicit operator EventCallback<T>(CallbackComposer<T> instance) => instance.Get();

	readonly object?       _receiver;
	readonly Func<T, Task> _method;

	public CallbackComposer(Func<T, Task> method) : this(method.Target, method) {}

	public CallbackComposer(object? receiver, Func<T, Task> method)
	{
		_receiver = receiver;
		_method   = method;
	}

	public OperationCallbackComposer<T> UpdateActivity(IActivityReceiver receiver)
		=> UpdateActivity(receiver, ActivityOptions.Default);

	public OperationCallbackComposer<T> UpdateActivity(IActivityReceiver receiver, ActivityOptions options)
	{
		var body      = Start.A.Selection(_method).Then().Structure().Out();
		var operation = new ActivityAwareOperation<T>(body, receiver, options);
		return new(receiver, operation);
	}

	public OperationCallbackComposer<T> Handle<TReported>(IExceptions exceptions)
		=> Handle(exceptions, A.Type<TReported>());

	public OperationCallbackComposer<T> Handle(IExceptions exceptions, Type? reportedType = null)
	{
		var receiver  = _receiver.Verify();
		var operation = new ExceptionAwareOperation<T>(reportedType ?? receiver.GetType(), exceptions, _method);
		var result    = new OperationCallbackComposer<T>(receiver, operation);
		return result;
	}

	public EventCallback<T> Get() => EventCallback.Factory.Create(_receiver.Verify(), _method);

	public CallbackComposer<T> Hide() => new(EmptyReceiver.Default, Start.A.Selection(_method).Get);
}