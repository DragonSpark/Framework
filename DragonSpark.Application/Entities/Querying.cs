using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace DragonSpark.Application.Entities
{
	sealed class Querying<T> : IQuerying<T>
	{
		readonly IQueryable<T>       _queryable;
		readonly IAsyncEnumerable<T> _enumerable;

		public Querying(IQueryable<T> queryable) : this(queryable, queryable.AsAsyncEnumerable()) {}

		public Querying(IQueryable<T> queryable, IAsyncEnumerable<T> enumerable)
		{
			_queryable  = queryable;
			_enumerable = enumerable;
		}

		public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_queryable).GetEnumerator();

		public Type ElementType => _queryable.ElementType;

		public Expression Expression => _queryable.Expression;

		public IQueryProvider Provider => _queryable.Provider;

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
			=> _enumerable.GetAsyncEnumerator(cancellationToken);
	}
}