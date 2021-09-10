using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class StartWhere<TIn, T> : Where<TIn, T> where T : class
	{
		protected StartWhere(Expression<Func<T, bool>> where) : base(Set<TIn, T>.Default, where) {}

		public StartWhere(Expression<Func<TIn, T, bool>> where) : base(Set<TIn, T>.Default, where) {}
	}

	public class StartWhere<T> : Where<T> where T : class
	{
		protected StartWhere(Expression<Func<T, bool>> where) : base(Set<T>.Default, where) {}
	}
}