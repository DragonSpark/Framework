using System;
using System.Linq.Expressions;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	sealed class ConvertAlteration : Invocation0<Expression, Type, Expression>,
	                                 IAlteration<Expression>,
	                                 IActivateUsing<Type>
	{
		public ConvertAlteration(Type type) : base(Expression.Convert, type) {}
	}
}