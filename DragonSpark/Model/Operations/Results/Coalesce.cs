using DragonSpark.Compose;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

[UsedImplicitly]
public class Coalesce<T> : IResulting<T>
{
	readonly IResulting<T?> _first;
	readonly IResulting<T>  _second;

	protected Coalesce(IResulting<T?> first, IResulting<T> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<T> Get() => await _first.Off() ?? await _second.Off();
}