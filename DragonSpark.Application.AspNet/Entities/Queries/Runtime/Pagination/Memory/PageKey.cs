using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination.Memory;

public sealed class PageKey<T> : Key<PageInput>
{
	public static PageKey<T> Default { get; } = new();

	PageKey() : base(A.Type<T>(), x => $"{x.IncludeTotalCount}+{x.OrderBy}+{x.Filter}+{x.Partition}") {}
}