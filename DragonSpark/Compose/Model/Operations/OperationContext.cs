using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

public sealed class OperationContext<T> : Selector<T, ValueTask>
{
	public static implicit operator Operate<T>(OperationContext<T> instance) => instance.Get().Get;

	public static implicit operator Await<T>(OperationContext<T> instance) => instance.Get().Await;

	readonly ISelect<T, ValueTask> _subject;

	public OperationContext(ISelect<T, ValueTask> subject) : base(subject) => _subject = subject;

	public OperationContext<T> Append(ISelect<T, ValueTask> command) => Append(command.Await);

	public OperationContext<T> Append(ICommand<T> command) => Append(command.Execute);
	public OperationContext<T> Append(Action<T> command) => Append(Start.A.Command(command).Operation());
	public OperationContext<T> Append(Await<T> command)
		=> new OperationContext<T>(new Appending<T>(Get().Await, command));

	public OperationContext<T> Append(IOperation command) => Append(command.Await);
	public OperationContext<T> Append(Await command)
		=> new OperationContext<T>(new Termination<T>(Get().Await, command));

	public LogOperationContext<T, TParameter> Bind<TParameter>(ILogMessage<TParameter> log)
		=> new LogOperationContext<T, TParameter>(_subject, log);

	public OperationSelector Bind(IResult<ValueTask<T>> parameter) => Bind(parameter.Get);

	public OperationSelector Bind(Func<ValueTask<T>> parameter)
		=> new OperationSelector(new Binding<T>(this.Out(), parameter));


	public SelectedLogOperationExceptionContext<T, TOther> Use<TOther>(ILogException<TOther> log)
		=> new SelectedLogOperationExceptionContext<T, TOther>(_subject, log);

	public PolicyAwareLogOperationExceptionContext<T> Use(ILogException<T> log)
		=> new PolicyAwareLogOperationExceptionContext<T>(_subject, log);

	public OperationContext<T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

	public OperationContext<T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

	public OperationContext<T> Watching(Func<CancellationToken> token)
		=> new OperationContext<T>(new TokenAwareOperation<T>(Get(), token));

	public TaskSelector<T> Allocate() => new TaskSelector<T>(Get().Select(SelectTask.Default));


}