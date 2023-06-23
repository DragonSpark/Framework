using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Text;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public class MemoryAwarePages<T> : Selecting<PageInput, Page<T>>, IPages<T>
{
	protected MemoryAwarePages(IPages<T> previous, IMemoryCache memory, IFormatter<PageInput> formatter)
		: base(previous.Then().Store().In(memory).For(TimeSpan.FromMinutes(5)).Using(formatter)) {}
}