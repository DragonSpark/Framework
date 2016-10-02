using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Expressions
{
	abstract class InvocationFactoryBase<TParameter, TDelegate> : CompiledDelegateFactoryBase<TParameter, TDelegate> where TParameter : MethodBase
	{
		protected InvocationFactoryBase( Func<ExpressionBodyParameter<TParameter>, Expression> bodySource ) : this( Parameter.Default, bodySource ) {}
		protected InvocationFactoryBase( ParameterExpression expression, Func<ExpressionBodyParameter<TParameter>, Expression> bodySource ) : base( expression, bodySource ) {}
	}
}