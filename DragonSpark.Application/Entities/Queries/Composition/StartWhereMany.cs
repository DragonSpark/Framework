using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class StartWhereMany<TIn, T, TTo> : WhereMany<TIn, T, TTo> where T : class
	{
		protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		public StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<TIn, T, IEnumerable<TTo>>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		public StartWhereMany(Expression<Func<TIn, T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		public StartWhereMany(Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, IEnumerable<TTo>>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		public StartWhereMany(Expression<Func<TIn, T, bool>> where,
		                      Expression<Func<DbContext, TIn, T, IEnumerable<TTo>>> select)
			: base(Set<TIn, T>.Default, where, select) {}
	}

	public class StartWhereMany<T, TTo> : WhereMany<T, TTo> where T : class
	{
		protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
			: base(Set<T>.Default.Then(), where, select) {}
	}
}