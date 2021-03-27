using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Appended : IOperation
	{
		readonly Await _previous, _current;

		public Appended(Await previous, Await current)
		{
			_previous = previous;
			_current  = current;
		}

		public async ValueTask Get()
		{
			await _previous();
			await _current();
		}
	}

	public class Appended<T> : IOperation<T>
	{
		readonly Await<T> _first;
		readonly Await<T> _second;

		public Appended(IOperation<T> first, IOperation<T> second) : this(first.Await, second.Await) {}

		public Appended(Await<T> first, Await<T> second)
		{
			_first  = first;
			_second = second;
		}

		public async ValueTask Get(T parameter)
		{
			await _first(parameter);
			await _second(parameter);
		}
	}

	public class AppendedTo<T> : IOperation<T>
	{
		readonly Await<T> _first;
		readonly Await    _second;

		public AppendedTo(Await<T> first, Await second)
		{
			_first  = first;
			_second = second;
		}

		public async ValueTask Get(T parameter)
		{
			await _first(parameter);
			await _second();
		}
	}
}