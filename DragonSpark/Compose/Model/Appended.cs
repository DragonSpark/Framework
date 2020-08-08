using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	sealed class Appended<T> : IOperation<T>
	{
		readonly Await<T> _first;
		readonly Await<T> _second;

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
}