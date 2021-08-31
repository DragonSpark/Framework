using DragonSpark.Model;
using DragonSpark.Model.Results;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	public class Query<T> : Query<None, T>, IQuery<T>
	{
		protected Query(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}
	}

	public class Query<TIn, T> : Instance<Expression<Func<DbContext, TIn, IQueryable<T>>>>, IQuery<TIn, T>
	{
		protected Query(Expression<Func<DbContext, IQueryable<T>>> instance)
			: base((context, _) => instance.Invoke(context)) {}

		protected Query(Expression<Func<TIn, IQueryable<T>>> instance)
			: base((context, @in) => instance.Invoke(@in)) {}

		protected Query(Expression<Func<DbContext, TIn, IQueryable<T>>> instance) : base(instance) {}
	}

}