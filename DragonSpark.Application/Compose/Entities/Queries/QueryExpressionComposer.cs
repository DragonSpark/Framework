﻿using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Compilation;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class QueryExpressionComposer<TIn, T> : IResult<IQuery<TIn, T>>
	{
		readonly Expression<Func<DbContext, TIn, IQueryable<T>>> _subject;

		public QueryExpressionComposer(Expression<Func<DbContext, TIn, IQueryable<T>>> subject) => _subject = subject;

		public IForm<TIn, T> Compile() => Compiler<TIn, T>.Default.Get(_subject);

		public IQuery<TIn, T> Get() => new Query<TIn, T>(_subject);
	}
}