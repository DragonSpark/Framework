using AsyncUtilities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace DragonSpark.Application.Entities
{
	sealed class ProtectedQuery<T> : IQuerying<T> where T : class
	{
		readonly IQuerying<T>        _query;
		readonly IAsyncQueryProvider _provider;
		readonly AsyncLock           _lock;

		public ProtectedQuery(IQuerying<T> query, IAsyncQueryProvider provider, AsyncLock @lock)
		{
			_query    = query;
			_provider = provider;
			_lock     = @lock;
		}

		public Type ElementType => _query.ElementType;

		public Expression Expression => _query.Expression;

		public IQueryProvider Provider => _provider;

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
			=> new ProtectedEnumerator<T>(_query.GetAsyncEnumerator(cancellationToken), _lock);

		public IEnumerator<T> GetEnumerator() => _query.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _query.GetEnumerator();
	}
}