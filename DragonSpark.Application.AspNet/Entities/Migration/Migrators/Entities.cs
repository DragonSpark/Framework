using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Entities<T> : IQueryable<T>, IAsyncEnumerable<T>
{
	readonly IAsyncEnumerable<T> _source;

	public Entities(IAsyncEnumerable<T> source)
	{
		_source    = source ?? throw new ArgumentNullException(nameof(source));
		Provider   = new QueryProvider<T>(this);
		Expression = Expression.Constant(this);
	}

	public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
		=> _source.GetAsyncEnumerator(token);

	public IEnumerator<T> GetEnumerator() => _source.ToBlockingEnumerable().GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public Type ElementType => typeof(T);
	public Expression Expression { get; }
	public IQueryProvider Provider { get; }
}