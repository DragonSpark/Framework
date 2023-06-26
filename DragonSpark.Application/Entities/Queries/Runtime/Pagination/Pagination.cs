using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class Pagination<T> : IPagination<T>
{
	readonly IPaging<T> _paging;
	readonly IAny<T>    _any;
	readonly IPages<T>  _default;

	public Pagination(IAny<T> any, IPaging<T> previous) : this(any, previous, EmptyPages<T>.Default) {}

	public Pagination(IAny<T> any, IPaging<T> paging, IPages<T> @default)
	{
		_paging  = paging;
		_any     = any;
		_default = @default;
	}

	public async ValueTask<IPages<T>> Get(PagingInput<T> parameter)
	{
		var (owner, queries, _) = parameter;
		var result = await _any.Await(new(owner, queries)) ? _paging.Get(parameter) : _default;
		return result;
	}
}