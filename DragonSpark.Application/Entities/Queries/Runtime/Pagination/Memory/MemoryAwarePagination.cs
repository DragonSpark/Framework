﻿using DragonSpark.Application.Compose.Store.Operations;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination.Memory;

public class MemoryAwarePagination<T> : IPagination<T>
{
	readonly MemoryStoreProfile<PageInput> _profile;

	protected MemoryAwarePagination(MemoryStoreProfile<PageInput> profile) => _profile = profile;

	public IPages<T> Get(IPages<T> parameter) => new MemoryAwarePages<T>(parameter, _profile);
}