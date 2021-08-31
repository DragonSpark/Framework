using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class StartSelect<TIn, TFrom, TTo> : Select<TIn, TFrom, TTo> where TFrom : class
	{
		protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TIn, TFrom>.Default, select) {}
	}

	public class StartSelect<TFrom, TTo> : Select<TFrom, TTo> where TFrom : class
	{
		protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TFrom>.Default, select) {}
	}
}