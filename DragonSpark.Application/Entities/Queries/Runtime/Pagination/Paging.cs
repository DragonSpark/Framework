using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class Paging<T> : IPaging<T>
{
	public static Paging<T> Default { get; } = new();

	Paging() {}

	public IPages<T> Get(PagingInput<T> parameter) => new Pages<T>(parameter.Queries, parameter.Compose);
}


// TODO

public class PageKey<T> : Key<PageInput>
{
	protected PageKey() : base(A.Type<T>(), x => $"{x.IncludeTotalCount}+{x.OrderBy}+{x.Filter}+{x.Partition}") {}
}

public class MemoryAwarePages<T> : Selecting<PageInput, Page<T>>, IPages<T>
{
	protected MemoryAwarePages(IPages<T> previous, IMemoryCache memory, IFormatter<PageInput> formatter)
		: base(previous.Then().Store().In(memory).For(TimeSpan.FromMinutes(5)).Using(formatter)) {}
}