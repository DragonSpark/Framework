using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public class PageKey<T> : Key<PageInput>
{
	protected PageKey() : base(A.Type<T>(), x => $"{x.IncludeTotalCount}+{x.OrderBy}+{x.Filter}+{x.Partition}") {}
}