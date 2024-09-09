﻿using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Appending = DragonSpark.Model.Operations.Appending;
using Await = DragonSpark.Model.Operations.Await;

namespace DragonSpark.Compose.Model.Operations;

public class OperationComposer<T> : Composer<T, ValueTask>
{
	public static implicit operator Operate<T>(OperationComposer<T> instance) => instance.Get().Get;

	public static implicit operator DragonSpark.Model.Operations.Await<T>(OperationComposer<T> instance)
		=> instance.Get().Await;

	readonly ISelect<T, ValueTask> _subject;

	public OperationComposer(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

	public OperationComposer<T> Append(ISelect<T, ValueTask> command) => Append(command.Await);

	public OperationComposer<T> Append(ICommand<T> command) => Append(command.Execute);

	public OperationComposer<T> Append(Action<T> command) => Append(Start.A.Command(command).Operation());

	public OperationComposer<T> Append(DragonSpark.Model.Operations.Await<T> command)
		=> new(new DragonSpark.Model.Operations.Appending<T>(Get().Await, command));

	public OperationComposer<T> Append(IOperation command) => Append(command.Await);

	public OperationComposer<T> Append(Await command)
		=> new(new DragonSpark.Model.Operations.Termination<T>(Get().Await, command));

	public LogOperationComposer<T, TParameter> Bind<TParameter>(ILogMessage<TParameter> log) => new(_subject, log);

	public OperationComposer Bind(IResult<ValueTask<T>> parameter) => Bind(parameter.Get);

	public OperationComposer Bind(Func<ValueTask<T>> parameter) => new(new Binding<T>(this.Out(), parameter));

	public SelectedLogOperationExceptionComposer<T, TOther> Use<TOther>(ILogException<TOther> log) => new(_subject, log);

	public PolicyAwareLogOperationExceptionComposer<T> Use(ILogException<T> log) => new(_subject, log);

	public OperationComposer<T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

	public OperationComposer<T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

	public OperationComposer<T> Watching(Func<CancellationToken> token) => new(new TokenAwareOperation<T>(Get(), token));

	public TaskComposer<T> Allocate() => new(Get().Select(SelectTask.Default));

	public OperationComposer<T> Delayed(TimeSpan by) => new(new Delay<T>(_subject.Then().Out(), by));
}

public class OperationComposer : ResultComposer<ValueTask>
{
	public static implicit operator Operate(OperationComposer instance) => instance.Get().Get;

	public static implicit operator Await(OperationComposer instance) => instance.Get().Await;

	public OperationComposer(IResult<ValueTask> instance) : base(instance) {}

	public OperationComposer Append(Await next) => new(new Appending(Get().Await, next));

	public OperationComposer Append(Operate next) => new(new AppendedOperate(Get().Get, next));

	public AllocatedOperationComposer Allocate() => new(Select(x => x.AsTask()).Get());

	public OperationComposer Disperse() => new(new DelayedDisperse(new Operation(this)).Then().Bind().Get());
}