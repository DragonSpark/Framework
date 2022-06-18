using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class Any<T> : IAny<T>
{
	public static Any<T> Default { get; } = new();

	Any() : this(DefaultAny<T>.Default) {}

	readonly Materialize.IAny<T> _any;

	public Any(Materialize.IAny<T> any) => _any = any;

	public async ValueTask<bool> Get(AnyInput<T> parameter)
	{
		using var query  = await parameter.Source.Await();
		var       result = await _any.Await(query.Subject);
		return result;
	}
}