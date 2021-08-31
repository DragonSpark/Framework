﻿using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class Introduce<TIn, TFrom, TOther, TTo> : Query<TIn, TTo>
	{
		public Introduce(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, TIn, IQueryable<TOther>>> other,
		                 Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
			                 select)
			: base((context, @in)
				       => select.Invoke(context, @in, from.Invoke(context, @in), other.Invoke(context, @in))) {}

		public Introduce(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, TIn, IQueryable<TOther>>> other,
		                 Expression<Func<TIn, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base((context, @in) => select.Invoke(@in, from.Invoke(context, @in), other.Invoke(context, @in))) {}
	}

	public class Introduce<TFrom, TOther, TTo> : Introduce<None, TFrom, TOther, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(
			Introduce<TFrom, TOther, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => other.Invoke(context),
			       (context, _, f, o) => select.Invoke(context, f, o)) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base((context, _) => from.Invoke(context), (context, _) => other.Invoke(context),
			       (context, _, f, o) => select.Invoke(f, o)) {}
	}
}