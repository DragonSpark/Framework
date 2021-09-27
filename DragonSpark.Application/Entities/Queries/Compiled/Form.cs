﻿using DragonSpark.Application.Entities.Editing;
using DragonSpark.Application.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled
{
	sealed class Form<TIn, T> : DragonSpark.Model.Selection.Select<In<TIn>, IAsyncEnumerable<T>>, IForm<TIn, T>
	{
		public Form(IQuery<TIn, T> query) : this(query.Get()) {}

		public Form(Expression<Func<DbContext, TIn, IQueryable<T>>> expression) : base(expression.Then().Compile()) {}
	}
}