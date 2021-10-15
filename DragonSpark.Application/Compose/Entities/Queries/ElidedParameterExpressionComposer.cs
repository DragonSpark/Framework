using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class ElidedParameterExpressionComposer<T>
	: DragonSpark.Model.Results.Instance<Expression<Func<DbContext, IQueryable<T>>>>
{
	public static implicit operator Expression<Func<DbContext, None, IQueryable<T>>>(
		ElidedParameterExpressionComposer<T> instance)
		=> instance.Accept();

	readonly Expression<Func<DbContext, IQueryable<T>>> _subject;

	public ElidedParameterExpressionComposer(Expression<Func<DbContext, IQueryable<T>>> subject) : base(subject)
		=> _subject = subject;

	public Expression<Func<DbContext, None, IQueryable<T>>> Accept() => (context, _) => _subject.Invoke(context);

	public Expression<Func<DbContext, TParameter, IQueryable<T>>> Accept<TParameter>()
		=> (context, _) => _subject.Invoke(context);

	public IQuery<T> Promote() => new Query<T>(_subject);
}