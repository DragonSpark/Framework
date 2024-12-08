using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Compose.Entities.Queries;

public sealed class InstanceComposer<T>
{
	readonly IInstance<T> _subject;

	public InstanceComposer(IInstance<T> subject) => _subject = subject;

	public IQuery<T> Query<U>() where U : class => new SelectInstance<U, T>(_subject);

	public Expression<Func<DbContext, None, IQueryable<T>>> Get<U>() where U : class => Query<U>().Get();
}

public sealed class InstanceComposer<TIn, T>
{
	readonly IInstance<TIn, T> _subject;

	public InstanceComposer(IInstance<TIn, T> subject) => _subject = subject;

	public IQuery<TIn, T> Query<U>() where U : class => new SelectInstance<TIn, U, T>(_subject);
	public Expression<Func<DbContext, TIn, IQueryable<T>>> Get<U>() where U : class => Query<U>().Get();
}