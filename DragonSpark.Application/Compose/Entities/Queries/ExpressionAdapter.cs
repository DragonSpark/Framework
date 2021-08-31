using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class ExpressionAdapter<T>
	{
		public static implicit operator Expression<Func<DbContext, None, IQueryable<T>>>(ExpressionAdapter<T> instance)
			=> instance.Accept();

		readonly Expression<Func<DbContext, IQueryable<T>>> _subject;

		public ExpressionAdapter(Expression<Func<DbContext, IQueryable<T>>> subject) => _subject = subject;

		public Expression<Func<DbContext, None, IQueryable<T>>> Accept() => (context, _) => _subject.Invoke(context);

		public Expression<Func<DbContext, TParameter, IQueryable<T>>> Accept<TParameter>()
			=> (context, _) => _subject.Invoke(context);
	}
}
