using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Invocation.Expressions;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Compose.Model.Selection;

public class ExpressionComposer<_, T> : Composer<_, Expression<T>>
{
	public ExpressionComposer(ISelect<_, Expression<T>> subject) : base(subject) {}

	public Composer<_, T> Compile() => Select(Compiler<T>.Default);
}

public class ExpressionComposer<_> : Composer<_, Expression>
{
	public ExpressionComposer(ISelect<_, Expression> subject) : base(subject) {}

	public ExpressionComposer<_, TTo> Select<TTo>(ISelect<Expression, Expression<TTo>> select)
		=> Select(select.Get);

	public ExpressionComposer<_, TTo> Select<TTo>(Func<Expression, Expression<TTo>> select) => new(Get().Select(select));
}