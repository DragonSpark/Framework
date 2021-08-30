using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	class Class2 {}

	public sealed class QueryAdapter<T>
	{
		readonly IQuery<T> _subject;

		public QueryAdapter(IQuery<T> subject) => _subject = subject;

		public Expression<Func<DbContext, TFor, IQueryable<T>>> For<TFor>()
		{
			var instance = _subject.Get();
			return (c, _) => instance.Invoke(c, None.Default);
		}

		public Expression<Func<DbContext, IQueryable<T>>> Without()
		{
			var instance = _subject.Get();
			return c => instance.Invoke(c, None.Default);
		}
	}
}