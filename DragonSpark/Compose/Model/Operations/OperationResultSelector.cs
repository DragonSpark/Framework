using AsyncUtilities;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ValueTask = System.Threading.Tasks.ValueTask;

namespace DragonSpark.Compose.Model.Operations
{
	public class OperationResultSelector<T> : ResultContext<ValueTask<T>>
	{
		public static implicit operator OperateOf<T>(OperationResultSelector<T> instance) => instance.Get().Get;

		public static implicit operator AwaitOf<T>(OperationResultSelector<T> instance) => instance.Get().Await;

		public OperationResultSelector(IResult<ValueTask<T>> instance) : base(instance) {}

		public OperationResultSelector<T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

		public OperationResultSelector<T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

		public OperationResultSelector<T> Watching(Func<CancellationToken> token)
			=> new(new TokenAwareOperationResult<T>(Get(), token));

		public OperationResultSelector<TTo> Select<TTo>(Func<T, TTo> select)
			=> new(new OperationResulting<T, TTo>(Get().Get, select));

		public AllocatedOperationResultSelector<T> Allocate() => new(Get().Then().Select(x => x.AsTask()).Get());
	}

	public class OperationResultSelector<_, T> : Selector<_, ValueTask<T>>
	{
		public static implicit operator Operate<_, T>(OperationResultSelector<_, T> instance) => instance.Get().Get;

		public static implicit operator Await<_, T>(OperationResultSelector<_, T> instance) => instance.Get().Await;

		public static implicit operator Await<_>(OperationResultSelector<_, T> instance) => instance.Terminate();

		public OperationResultSelector(ISelect<_, ValueTask<T>> subject) : base(subject) {}

		public TaskSelector<_, T> Allocate() => new(Get().Select(SelectTask<T>.Default));

		public OperationResultSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public OperationResultSelector<_, TTo> Select<TTo>(ISelect<T, ValueTask<TTo>> select) => Select(select.Get);

		public OperationResultSelector<_, TTo> Select<TTo>(Func<T, ValueTask<TTo>> select)
		{
			var subject = Get().Select(new OperationSelect<T, ValueTask<TTo>>(@select)).Select(Assuming<TTo>.Default);
			var result  = new OperationResultSelector<_, TTo>(subject);
			return result;
		}

		public new OperationResultSelector<_, (_ In, T Out)> Introduce()
			=> new DragonSpark.Model.Operations.Introduce<_, T>(Get()).Then();

		public OperationResultSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
			=> new(Get().Select(new OperationSelect<T, TTo>(select)));

		public OperationResultSelector<_, T> Protecting() => Protecting(new AsyncLock());

		public OperationResultSelector<_, T> Protecting(AsyncLock @lock)
			=> new(new Protecting<_, T>(Get().Await, @lock));

		public OperationResultSelector<_, T> Configure(Action<T> configure)
			=> new(Get().Select(new Configure<T>(configure)));

		public OperationContext<_> Terminate(ISelect<T, ValueTask> command) => Terminate(command.Get);

		public new OperationContext<_> Terminate() => Terminate(_ => ValueTask.CompletedTask);

		public OperationContext<_> Terminate(Func<T, ValueTask> command)
			=> new(Get().Select(new OperationSelect<T>(command)));

		public OperationContext<_> Terminate(Action<T> command) => new(Get().Select(new InvokingParameter<T>(command)));

		public OperationResultSelector<_, T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

		public OperationResultSelector<_, T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

		public OperationResultSelector<_, T> Watching(Func<CancellationToken> token)
			=> new(new TokenAwareSelecting<_, T>(Get(), token));
	}

	sealed class TokenAwareOperationResult<T> : IResulting<T>
	{
		readonly IResult<ValueTask<T>>   _operation;
		readonly Func<CancellationToken> _token;

		public TokenAwareOperationResult(IResult<ValueTask<T>> operation, Func<CancellationToken> token)
		{
			_operation = operation;
			_token     = token;
		}

		public async ValueTask<T> Get()
		{
			var token = _token();
			token.ThrowIfCancellationRequested();
			var result = await _operation.Await();
			token.ThrowIfCancellationRequested();
			return result;
		}
	}

	sealed class TokenAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelect<TIn, ValueTask<TOut>> _operation;
		readonly Func<CancellationToken>       _token;

		public TokenAwareSelecting(ISelect<TIn, ValueTask<TOut>> operation, Func<CancellationToken> token)
		{
			_operation = operation;
			_token     = token;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var token = _token();
			token.ThrowIfCancellationRequested();
			var result = await _operation.Await(parameter);
			token.ThrowIfCancellationRequested();
			return result;
		}
	}
}