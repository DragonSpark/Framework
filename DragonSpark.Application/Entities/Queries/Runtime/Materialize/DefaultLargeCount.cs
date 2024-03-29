﻿using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

sealed class DefaultLargeCount<T> : ILargeCount<T>
{
	public static DefaultLargeCount<T> Default { get; } = new();

	DefaultLargeCount() {}

	public async ValueTask<ulong> Get(IQueryable<T> parameter)
	{
		var count  = await parameter.LongCountAsync().ConfigureAwait(false);
		var result = count.Grade();
		return result;
	}
}