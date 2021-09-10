using DragonSpark.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class StartWhereSelection<T, TTo> : StartWhereSelection<None, T, TTo> where T : class {
		protected StartWhereSelection(Expression<Func<T, bool>> @where, Expression<Func<IQueryable<T>, IQueryable<TTo>>> @select)
			: base(@where, @select) {}

		protected StartWhereSelection(Expression<Func<T, bool>> @where, Expression<Func<None, IQueryable<T>, IQueryable<TTo>>> @select)
			: base(@where, @select) {}
	}

	public class StartWhereSelection<TIn, T, TTo> : WhereSelection<TIn, T, TTo> where T : class
	{
		protected StartWhereSelection(Expression<Func<T, bool>> @where,
		                              Expression<Func<IQueryable<T>, IQueryable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		protected StartWhereSelection(Expression<Func<T, bool>> @where,
		                              Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		protected StartWhereSelection(Expression<Func<TIn, T, bool>> @where,
		                              Expression<Func<IQueryable<T>, IQueryable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}

		protected StartWhereSelection(Expression<Func<TIn, T, bool>> @where,
		                              Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> @select)
			: base(Set<TIn, T>.Default, @where, @select) {}
	}
}