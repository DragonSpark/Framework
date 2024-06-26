﻿using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

sealed class DefaultLargeCount<T> : ILargeCount<T>
{
	public static DefaultLargeCount<T> Default { get; } = new();

	DefaultLargeCount() {}

	public async ValueTask<ulong> Get(TokenAware<IQueryable<T>> parameter)
	{
		var (subject, token) = parameter;
		var count  = await subject.LongCountAsync(token).ConfigureAwait(false);
		var result = count.Grade();
		return result;
	}
}