using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Invocation.Expressions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Activation;

sealed class ParameterConstructors<TIn, TOut> : Invocation.Expressions.Delegates<ConstructorInfo, Func<TIn, TOut>>
{
	public static ParameterConstructors<TIn, TOut> Default { get; }
		= new ParameterConstructors<TIn, TOut>();

	ParameterConstructors() : this(ConstructorExpressions<TIn>.Default) {}

	public ParameterConstructors(ISelect<ConstructorInfo, Expression> select)
		: base(select, Parameter<TIn>.Default.Get()) {}
}