﻿using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

sealed class DefaultToArray<T> : IToArray<T>
{
	public static DefaultToArray<T> Default { get; } = new();

	DefaultToArray() {}

	public async ValueTask<Array<T>> Get(TokenAware<IQueryable<T>> parameter)
	{
		var (queryable, token) = parameter;
		return await queryable.ToArrayAsync(token).ConfigureAwait(false);
	}
}