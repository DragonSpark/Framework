﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartSelectMany<TIn, TFrom, TTo> : SelectMany<TIn, TFrom, TTo> where TFrom : class
{
	protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select)
		: base(Set<TIn, TFrom>.Default, select) {}

	public StartSelectMany(Expression<Func<TIn, TFrom, IEnumerable<TTo>>> select)
		: base(Set<TIn, TFrom>.Default, select) {}
}

public class StartSelectMany<TFrom, TTo> : SelectMany<TFrom, TTo> where TFrom : class
{
	protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select)
		: base(Set<TFrom>.Default.Then(), select) {}
}