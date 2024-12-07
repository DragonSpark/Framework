using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class PlaceholderParameterExpressionComposer<T>
{
	public static implicit operator Expression<Func<DbContext, T>>(PlaceholderParameterExpressionComposer<T> instance)
		=> instance.Elide();

	public static implicit operator Expression<Func<DbContext, None, T>>(
		PlaceholderParameterExpressionComposer<T> instance)
		=> instance._subject;

	readonly Expression<Func<DbContext, None, T>> _subject;

	public PlaceholderParameterExpressionComposer(Expression<Func<DbContext, None, T>> subject) => _subject = subject;

	public Expression<Func<DbContext, TFor, T>> Accept<TFor>() => (c, _) => _subject.Invoke(c, None.Default);

	public ElidedParameterExpressionComposer<T> Elide() => new (c => _subject.Invoke(c, None.Default));
}