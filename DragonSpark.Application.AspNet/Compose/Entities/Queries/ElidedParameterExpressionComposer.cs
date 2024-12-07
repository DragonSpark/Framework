using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class ElidedParameterExpressionComposer<T>
	: DragonSpark.Model.Results.Instance<Expression<Func<DbContext, T>>>
{
	public static implicit operator Expression<Func<DbContext, None, T>>(
		ElidedParameterExpressionComposer<T> instance)
		=> instance.Accept();

	readonly Expression<Func<DbContext, T>> _subject;

	public ElidedParameterExpressionComposer(Expression<Func<DbContext, T>> subject) : base(subject)
		=> _subject = subject;

	public Expression<Func<DbContext, None, T>> Accept() => (context, _) => _subject.Invoke(context);

	public Expression<Func<DbContext, TParameter, T>> Accept<TParameter>()
		=> (context, _) => _subject.Invoke(context);
}