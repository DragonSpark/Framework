using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class WhenBoth<T> : IDepending<T>
{
	readonly ISelecting<T, bool> _first, _second;

	protected WhenBoth(ISelecting<T, bool> first, ISelecting<T, bool> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<bool> Get(T parameter)
		=> await _first.Await(parameter) && await _second.Await(parameter);
}