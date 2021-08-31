using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class QueryExpressionAdapter<T>
	{


		readonly Expression<Func<DbContext, None, IQueryable<T>>> _subject;

		public QueryExpressionAdapter(Expression<Func<DbContext, None, IQueryable<T>>> subject) => _subject = subject;

		public Expression<Func<DbContext, TFor, IQueryable<T>>> For<TFor>()
			=> (c, _) => _subject.Invoke(c, None.Default);

		public Expression<Func<DbContext, IQueryable<T>>> Remove() => c => _subject.Invoke(c, None.Default);
	}
}