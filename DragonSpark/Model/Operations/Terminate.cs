using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Terminate<T> : IOperation<T>
	{
		readonly Await<T> _first;
		readonly Await    _second;

		public Terminate(Await<T> first, Await second)
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