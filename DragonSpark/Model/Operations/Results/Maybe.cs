using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

[UsedImplicitly]
public class Maybe<T> : IResulting<T?>
{
	readonly AwaitOf<T?> _first, _second;

	public Maybe(IResulting<T?> first, IResulting<T?> second) : this(first.Await, second.Await) {}

	public Maybe(AwaitOf<T?> first, AwaitOf<T?> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<T?> Get() => await _first() ?? await _second();
}