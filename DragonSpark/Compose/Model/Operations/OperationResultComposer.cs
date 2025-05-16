using AsyncUtilities;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ValueTask = System.Threading.Tasks.ValueTask;

namespace DragonSpark.Compose.Model.Operations;

public class OperationResultComposer<T> : ResultComposer<ValueTask<T>>
{
	public static implicit operator OperateOf<T>(OperationResultComposer<T> instance) => instance.Get().Get;

	public static implicit operator DragonSpark.Model.Operations.AwaitOf<T>(OperationResultComposer<T> instance)
		=> instance.Get().Off;

	public OperationResultComposer(IResult<ValueTask<T>> instance) : base(instance) {}

	public OperationComposer Terminate(ISelect<T, ValueTask> command) => Terminate(command.Get);

	public OperationComposer Terminate() => Terminate(_ => ValueTask.CompletedTask);

	public OperationComposer Terminate(Func<T, ValueTask> command) => new(new ConfiguredTermination<T>(Get(), command));

	public OperationComposer Terminate(Action<T> command)
		=> new(new ConfiguredTermination<T>(Get(), Start.A.Command(command).Operation()));

	public OperationResultComposer<T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

	public OperationResultComposer<T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

	public OperationResultComposer<T> Watching(Func<CancellationToken> token)
		=> new(new TokenAwareOperationResult<T>(Get(), token));

	public OperationResultComposer<TTo> Select<TTo>(ISelecting<T, TTo> select) => Select(select.Get);

	public OperationResultComposer<TTo> Select<TTo>(Func<T, ValueTask<TTo>> select)
		=> new(new SelectingResult<T, TTo>(Get().Out(), select));

	public OperationResultComposer<TTo> Select<TTo>(Func<T, TTo> select)
		=> new(new OperationResulting<T, TTo>(Get().Get, select));

	public AllocatedResultComposer<T> Allocate() => new(Get().Then().Select(x => x.AsTask()).Get());

	public OperationResultComposer<T> Protecting() => Protecting(new AsyncLock());

	public OperationResultComposer<T> Protecting(AsyncLock @lock)
		=> new(new DragonSpark.Model.Operations.Results.Protecting<T>(Get(), @lock));
}

public class OperationResultComposer<_, T> : Composer<_, ValueTask<T>>
{
	public static implicit operator Operate<_, T>(OperationResultComposer<_, T> instance) => instance.Get().Get;

	public static implicit operator DragonSpark.Model.Operations.Await<_, T>(OperationResultComposer<_, T> instance)
		=> instance.Get().Off;

	public static implicit operator DragonSpark.Model.Operations.Await<_>(OperationResultComposer<_, T> instance)
		=> instance.Terminate();

	public OperationResultComposer(ISelect<_, ValueTask<T>> subject) : base(subject) {}

	public TaskComposer<_, T> Allocate() => new(Get().Select(SelectTask<T>.Default));

	public OperationResultComposer<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

	public OperationResultComposer<_, TTo> Select<TTo>(ISelect<T, ValueTask<TTo>> select) => Select(select.Get);

	public OperationResultComposer<_, TTo> Select<TTo>(Func<T, ValueTask<TTo>> select)
	{
		var subject = new Selecting<_, T, TTo>(Get().Get, select);
		var result  = new OperationResultComposer<_, TTo>(subject);
		return result;
	}

	public OperationResultComposer<_, TTo> Select<TTo>(Func<T, TTo> select)
		=> new(Get().Select(new OperationSelect<T, TTo>(select)));

	public new OperationResultComposer<_, (_ In, T Out)> Introduce()
		=> new DragonSpark.Model.Operations.Selection.Introduce<_, T>(Get()).Then();

	public OperationResultComposer<_, T> Using(CancellationToken parameter)
		=> new(new AssignedToken<_, T>(Get(), parameter));

	public OperationResultComposer<Stop<_>, Stop<T>> Stop() => new(new StopAdapter<_, T>(Get()));

	public OperationResultComposer<T> Bind(IResult<ValueTask<_>> parameter) => Bind(parameter.Get);

	public OperationResultComposer<T> Bind(Func<ValueTask<_>> parameter)
		=> new(new Binding<_, T>(this.Out(), parameter));

	public OperationResultComposer<_, T> Protecting() => Protecting(new AsyncLock());

	public OperationResultComposer<_, T> Protecting(AsyncLock @lock) => new(new Protecting<_, T>(Get().Off, @lock));

	public OperationResultComposer<_, T> Configure(Action<T> configure)
		=> new(Get().Select(new Configure<T>(configure)));

	public OperationComposer<_> Terminate(ISelect<T, ValueTask> command) => Terminate(command.Get);

	public new OperationComposer<_> Terminate() => Terminate(_ => ValueTask.CompletedTask);

	public OperationComposer<_> Terminate(Func<T, ValueTask> command)
		=> new(Get().Select(new OperationSelect<T>(command)));

	public OperationComposer<_> Terminate(Action<T> command) => new(Get().Select(new InvokingParameter<T>(command)));

	public OperationResultComposer<_, T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

	public OperationResultComposer<_, T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

	public OperationResultComposer<_, T> Watching(Func<CancellationToken> token)
		=> new(new TokenAwareSelecting<_, T>(Get(), token));
}