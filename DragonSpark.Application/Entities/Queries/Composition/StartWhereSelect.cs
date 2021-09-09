﻿using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class StartWhereSelect<TIn, T, TTo> : WhereSelect<TIn, T, TTo> where T : class
	{
		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<TIn, T, TTo>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		protected StartWhereSelect(Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<TIn, T>.Default, where, select) {}

		protected StartWhereSelect(Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, TTo>> select)
			: base(Set<TIn, T>.Default, where, select) {}
	}

	public class StartWhereSelect<T, TTo> : WhereSelect<T, TTo> where T : class
	{
		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<T>.Default.Then(), where, select) {}
	}
}