using DragonSpark.Model.Operations;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class OperationView<T> : IOperationResult<T>
	{
		readonly IOperationResult<T>           _view;
		readonly ConcurrentStack<ValueTask<T>> _operations;

		public OperationView(IOperationResult<T> view) : this(view, new ConcurrentStack<ValueTask<T>>()) {}

		public OperationView(IOperationResult<T> view, ConcurrentStack<ValueTask<T>> operations)
		{
			_view       = view;
			_operations = operations;
		}

		public bool IsActive => _operations.Count > 0;

		public async ValueTask<T> Get()
		{
			var operation = _view.Get();
			_operations.Push(operation);
			var result = await operation;

			_operations.TryPop(out _);

			return result;
		}
	}

	public sealed class OperationView<TIn, TOut> : IOperationResult<TIn, TOut>
	{
		readonly IOperationResult<TIn, TOut>      _view;
		readonly ConcurrentStack<ValueTask<TOut>> _operations;

		public OperationView(IOperationResult<TIn, TOut> view) : this(view, new ConcurrentStack<ValueTask<TOut>>()) {}

		public OperationView(IOperationResult<TIn, TOut> view, ConcurrentStack<ValueTask<TOut>> operations)
		{
			_view       = view;
			_operations = operations;
		}

		public bool IsActive => _operations.Count > 0;

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var operation = _view.Get(parameter);
			_operations.Push(operation);
			var result = await operation;

			_operations.TryPop(out _);

			return result;
		}
	}
}