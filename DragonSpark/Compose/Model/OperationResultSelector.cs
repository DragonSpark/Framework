using AsyncUtilities;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ValueTask = System.Threading.Tasks.ValueTask;

namespace DragonSpark.Compose.Model
{
	public class OperationResultSelector<T> : ResultContext<ValueTask<T>>
	{
		public static implicit operator OperateOf<T>(OperationResultSelector<T> instance) => instance.Get().Get;

		public static implicit operator AwaitOf<T>(OperationResultSelector<T> instance) => instance.Get().Await;

		public OperationResultSelector(IResult<ValueTask<T>> instance) : base(instance) {}

		public OperationResultSelector<T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

		public OperationResultSelector<T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

		public OperationResultSelector<T> Watching(Func<CancellationToken> token)
			=> new OperationResultSelector<T>(new TokenAwareOperationResult<T>(Get(), token));
	}

	public class OperationResultSelector<_, T> : Selector<_, ValueTask<T>>
	{
		public static implicit operator Operate<_, T>(OperationResultSelector<_, T> instance) => instance.Get().Get;

		public static implicit operator Await<_, T>(OperationResultSelector<_, T> instance) => instance.Get().Await;

		public static implicit operator Await<_>(OperationResultSelector<_, T> instance) => instance.Terminate();

		public OperationResultSelector(ISelect<_, ValueTask<T>> subject) : base(subject) {}

		public TaskSelector<_, T> Demote() => new TaskSelector<_, T>(Get().Select(SelectTask<T>.Default));

		public OperationResultSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

		public OperationResultSelector<_, TTo> Select<TTo>(ISelect<T, ValueTask<TTo>> select)
			=> Select(select.Get);

		public OperationResultSelector<_, TTo> Select<TTo>(Func<T, ValueTask<TTo>> select)
		{
			var subject = Get().Select(new OperationSelect<T, ValueTask<TTo>>(@select)).Select(Assuming<TTo>.Default);
			var result  = new OperationResultSelector<_, TTo>(subject);
			return result;
		}

		public OperationResultSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
			=> new OperationResultSelector<_, TTo>(Get().Select(new OperationSelect<T, TTo>(select)));

		public OperationResultSelector<_, T> Protecting() => Protecting(new AsyncLock());

		public OperationResultSelector<_, T> Protecting(AsyncLock @lock)
			=> new OperationResultSelector<_, T>(new Protecting<_, T>(Get().Await, @lock));


		public OperationContext<_> Terminate(ISelect<T, ValueTask> command) => Terminate(command.Get);

		public new OperationContext<_> Terminate() => Terminate(x => ValueTask.CompletedTask);
		public OperationContext<_> Terminate(Func<T, ValueTask> command)
			=> new OperationContext<_>(Get().Select(new OperationSelect<T>(command)));

		public OperationResultSelector<_, T> Watching(CancellationToken token) => Watching(Start.A.Result(token));

		public OperationResultSelector<_, T> Watching(IResult<CancellationToken> token) => Watching(token.Get);

		public OperationResultSelector<_, T> Watching(Func<CancellationToken> token)
			=> new OperationResultSelector<_, T>(new TokenAwareSelecting<_, T>(Get(), token));
	}

	sealed class TokenAwareOperationResult<T> : IResulting<T>
	{
		readonly IResult<ValueTask<T>> _operation;
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
		readonly Func<CancellationToken>     _token;

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