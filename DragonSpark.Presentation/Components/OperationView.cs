using DragonSpark.Model.Operations;
using System.Collections.Concurrent;
using System.Threading.Tasks;
// ReSharper disable PossibleMultipleConsumption

namespace DragonSpark.Presentation.Components
{
	public sealed class OperationView<T> : IResulting<T>
	{
		readonly IResulting<T> _view;
		readonly ConcurrentStack<ValueTask<T>> _operations;

		public OperationView(IResulting<T> view) : this(view, new ConcurrentStack<ValueTask<T>>()) {}

		public OperationView(IResulting<T> view, ConcurrentStack<ValueTask<T>> operations)
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

	public sealed class OperationView<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut>      _view;
		readonly ConcurrentStack<ValueTask<TOut>> _operations;

		public OperationView(ISelecting<TIn, TOut> view) : this(view, new ConcurrentStack<ValueTask<TOut>>()) {}

		public OperationView(ISelecting<TIn, TOut> view, ConcurrentStack<ValueTask<TOut>> operations)
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