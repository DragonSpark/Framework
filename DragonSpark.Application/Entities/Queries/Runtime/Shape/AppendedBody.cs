using DragonSpark.Compose;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public class AppendedBody<T> : IBody<T>
{
	readonly IBody<T> _first;
	readonly IBody<T> _second;

	public AppendedBody(IBody<T> first, IBody<T> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var first  = await _first.Await(parameter);
		var result = await _second.Await(new(parameter.Input, first));
		return result;
	}
}