using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime.Invocation;
using DragonSpark.Runtime.Invocation.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Reflection.Types;

class ActivationDelegates<T> : ReferenceValueTable<Type, T> where T : Delegate
{
	public ActivationDelegates(params Type[] parameters) : this(new ActivateExpressions(parameters)) {}

	public ActivationDelegates(IActivateExpressions expressions)
		: this(expressions, expressions.Parameters.Get().Open()) {}

	public ActivationDelegates(ISelect<Type, Expression> select, params ParameterExpression[] expressions)
		: base(new Lambda(expressions).Select(Compiler<T>.Default).To(select.Select).Get) {}

	sealed class Lambda : Invocation0<Expression, IEnumerable<ParameterExpression>, Expression<T>>
	{
		public Lambda(IEnumerable<ParameterExpression> parameter) : base(Expression.Lambda<T>, parameter) {}
	}
}